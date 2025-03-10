<script setup lang="ts">
import { ref } from 'vue';
import type Produto from '../../../model/Produto';
import { useToast } from 'vue-toastification';
import { deleteProduct, fetchProducts, updateProduct } from '../../../api/produtos';
import { DEFAULT_CURRENCY_PROPS, formatCurrency } from '../../../utils';

const toast = useToast();

const filter = ref("");
const lastFilter = ref("");

const previousPage = ref<Produto[]>([]);
const currentPage = ref<Produto[]>([]);
const nextPage = ref<Produto[]>([]);

const page = ref(1);

const editingProduct = ref<Produto | undefined>(undefined);
const editingDescription = ref("");
const editingPrice = ref("");

const waitingRemoval = ref<Produto | undefined>(undefined) 

const currencyProps = ref(DEFAULT_CURRENCY_PROPS);

function goForwards() {
  if(lastFilter.value != filter.value) {
    page.value = 1;
  }
  page.value++;
  previousPage.value = [...currentPage.value];
  currentPage.value = nextPage.value;
  fetchProducts(page.value + 1, filter.value, (r) => {
    if(r.status != 200) {
      r.errors.forEach(x => toast.error(x));
      toast.error(r.message);
      return;
    }
    nextPage.value = r.data!;
  })
}

function goBackwards() {
  if(page.value == 1) {
    return;
  }
  if(lastFilter.value != filter.value) {
    page.value = 1;
  }
  page.value--;
  nextPage.value = [...currentPage.value];
  currentPage.value = [...previousPage.value];
  fetchProducts(page.value - 1, filter.value, (r) => {
    if(r.status != 200) {
      r.errors.forEach(x => toast.error(x));
      toast.error(r.message);
      return;
    }
    previousPage.value = r.data!;
  })
}

function submit(first: boolean) {
  if(lastFilter.value != filter.value) {
    page.value = 1;
  }
  if(first) {
    fetchProducts(page.value, filter.value, (c) => {
      if(c.status != 200) {
        c.errors.forEach(x => toast.error(x));
        toast.error(c.message);
        return;
      }
      currentPage.value = c.data!;
      // Deixa a proxima pagina em cache
      fetchProducts(page.value + 1, filter.value, (c2) => {
        if(c2.status != 200) {
          c2.errors.forEach(x => toast.error(x));
          toast.error(c2.message);
          return;
        }
        nextPage.value = c2.data!;
      })
    })
    return;
  } 
}

function remove() {
  if(waitingRemoval == undefined) {
    return;
  }
  deleteProduct(waitingRemoval.value!.id, (resp) => {
    if(resp.status != 200) {
      resp.errors.forEach(x => toast.error(x));
      toast.error(resp.message);
      return;
    }
    toast.success(resp.message);
    waitingRemoval.value = undefined;
    submit(true);
  })
}

function startEditing(c: Produto) {
  editingProduct.value = c;
  editingDescription.value = c.description;
  editingPrice.value = formatCurrency(c.unitaryPrice);
}

function clearEdit() {
  editingProduct.value = undefined;
  editingDescription.value = "";
  editingPrice.value = "";
}

function saveEditing() {
  const parsedPrice = parseFloat(editingPrice.value.replace('R$', '').replace(/\./g, '').replace(',', '.').trim());
  const newProduct = {id: editingProduct.value!.id, description: editingDescription.value, unitaryPrice: parsedPrice};
  updateProduct(newProduct, (resp) => {
    if(resp.status != 200) {
      resp.errors.forEach(x => toast.error(x));
      toast.error(resp.message);
      return;
    }
    currentPage.value = currentPage.value.map(x => x.id == editingProduct.value?.id ? newProduct : x);
    toast.success(resp.message);
    clearEdit();
  })
}

submit(true);

</script>

<template>

  <div v-if="waitingRemoval != undefined" class="popup">
    <p>Você tem certeza que deseja deletar o produto <b>{{ waitingRemoval.description }}?</b> Por consequência, isso irá deletar todas as vendas associadas ao mesmo.</p>
    <div class="mx-auto gap-x-2 flex mt-4 justify-center">
      <button @click="remove" class="confirm-button">Confirmar</button>
      <button @click="waitingRemoval = undefined" class="cancel-button">Cancelar</button>
    </div>
  </div>

  <h1 class="text-2xl font-bold text-indigo-600">Listar Produtos</h1>
  <div class="mt-4">
    <div class="flex gap-x-2">
      <p class="text-sm font-light my-auto">Filtrar por descrição:</p>
      <input type="text" v-model="filter" :tabindex="0"/>
      <button class="c-button" :tabindex="1" @click="submit(true)">Buscar</button>
    </div>
    <div class="mt-4">
      <table class="w-full border-[1px] border-indigo-300">
        <tr>
          <th class="w-[10%] text-left">ID</th>
          <th class="w-[35%] text-left">Descrição</th>
          <th class="w-[35%] text-left">Preço</th>
          <th class="w-[20%] text-left">Ações</th>
        </tr>
        <tr v-for="(produto, i) in currentPage" class="border-b-[1px] border-indigo-300" :class="[i % 2 != 0 ? 'bg-indigo-50' : 'bg-white']">
          <td class="w-[10%]">{{ produto.id }}</td>
          <td class="w-[35%]">
            <input v-if="editingProduct?.id == produto.id" v-model="editingDescription" type="text" class="w-2/3"/>
            <p v-else>{{ produto.description }}</p>
          </td>
          <td class="w-[35%]">
            <input v-if="editingProduct?.id == produto.id" v-model.lazy="editingPrice" v-money3="currencyProps" class="w-2/3"/>
            <p v-else>{{ formatCurrency(produto.unitaryPrice) }}</p>
          </td>
          <td class="w-[20%]">
            <div v-if="editingProduct?.id != produto.id" class="flex gap-x-2">
              <img src="/edit-svgrepo-com.svg" class="action-button border-indigo-300 bg-indigo-50" @click="startEditing(produto)"/>
              <img src="/bin-svgrepo-com.svg" class="action-button border-red-300 bg-red-50" @click="waitingRemoval = produto"/>
            </div>
            <div v-else class="flex gap-x-2">
              <img src="/check-svgrepo-com.svg" class="action-button border-green-300 bg-green-50" @click="saveEditing"/>
              <img src="/cross-svgrepo-com.svg" class="action-button border-red-300 bg-red-50" @click="clearEdit"/>
            </div>
          </td>
        </tr>
      </table>
    </div>
    <div class="mt-4 justify-end flex gap-x-1">
      <button v-if="page > 1" @click="goBackwards" class="w-6 text-white bg-indigo-500 py-0.5 px-1 rounded-md cursor-pointer font-bold">&lt;</button>
      <p class="my-auto">{{ page }}</p>
      <button v-if="nextPage.length > 0" @click="goForwards" class="w-6 text-white bg-indigo-500 py-0.5 px-1 rounded-md cursor-pointer font-bold">&gt;</button>
    </div>
  </div>
</template>