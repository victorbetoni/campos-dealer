<script setup lang="ts">
import { ref } from 'vue';
import type Produto from '../../../model/Produto';
import { useToast } from 'vue-toastification';
import { fetchAllProducts } from '../../../api/produtos';
import { formatCurrency } from '../../../utils';
import type Venda from '../../../model/Venda';
import type Cliente from '../../../model/Cliente';
import { deleteSale, fetchSales, updateSale } from '../../../api/vendas';
import { fetchAllCustomers } from '../../../api/clientes';

const toast = useToast();

const custFilter = ref("");
const lastCustFilter = ref("");
const descFilter = ref("");
const lastDescFilter = ref("");

const previousPage = ref<Venda[]>([]);
const currentPage = ref<Venda[]>([]);
const nextPage = ref<Venda[]>([]);

const page = ref(1);

const editingSale = ref<Venda | undefined>(undefined);
const editingProduct = ref<Produto | undefined>(undefined);
const editingCustomer = ref<Cliente | undefined>(undefined);
const editingDate = ref("");
const editingQuantity = ref(0);
const waitingRemoval = ref<Venda | undefined>(undefined) 

const customers = ref<Cliente[]>([]);
const products = ref<Produto[]>([]);

function goForwards() {
  page.value++;
  previousPage.value = [...currentPage.value];
  currentPage.value = nextPage.value;
  fetchPage(1, x => nextPage.value = x);
}

function goBackwards() {
  if(page.value == 1) {
    return;
  }
  page.value--;
  nextPage.value = [...currentPage.value];
  currentPage.value = [...previousPage.value];
  fetchPage(-1, x => previousPage.value = x);
}

function fetchPage(offset: number, then: (v: Venda[]) => void) {
  if(lastCustFilter.value != custFilter.value || lastDescFilter.value != descFilter.value) {
    page.value = 1;
    lastCustFilter.value = custFilter.value;
    lastDescFilter.value = descFilter.value;
  }
  fetchSales(page.value + offset, custFilter.value, descFilter.value, (c) => {
      if(c.status != 200) {
        c.errors.forEach(x => toast.error(x));
        toast.error(c.message);
        return;
      }
      then(c.data!);
    })
}

function submit(first: boolean) {
  fetchPage(0, x => currentPage.value = x);
  if(first) {
    fetchPage(1, x => nextPage.value = x);
  }
}

function remove() {
  if(waitingRemoval == undefined) {
    return;
  }
  deleteSale(waitingRemoval.value!.id, (resp) => {
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

function startEditing(c: Venda) {
  editingSale.value = c;
  editingProduct.value = c.product;
  editingCustomer.value = c.customer;
  editingQuantity.value = c.quantity;
  let date = new Date(c.date);
  let year = date.getFullYear();
  let month = (date.getMonth() + 1).toString().padStart(2, '0');
  let day = date.getDate().toString().padStart(2, '0');
  let hours = date.getHours().toString().padStart(2, '0');
  let minutes = date.getMinutes().toString().padStart(2, '0');

  editingDate.value = `${year}-${month}-${day}T${hours}:${minutes}`;
}

function clearEdit() {
  editingSale.value = undefined;
}

function saveEditing() {
  if(new Date(editingDate.value) > new Date()) {
    toast.error("A data não pode estar no futuro.");
    return;
  }
  let newSale: Venda = ({
    id: editingSale.value!.id, 
    customer: editingCustomer.value!, 
    date: new Date(editingDate.value!).getTime(),
    product: editingProduct.value!,
    quantity: editingQuantity.value,
    total: editingQuantity.value! * editingProduct.value!.unitaryPrice, // ignorado pelo backend
    unitaryValue: editingProduct.value!.unitaryPrice // ignorado pelo backend 
  })
  updateSale(newSale, (resp) => {
    if(resp.status != 200) {
      resp.errors.forEach(x => toast.error(x));
      toast.error(resp.message);
      return;
    }
    currentPage.value = currentPage.value.map(x => x.id == editingSale.value?.id ? newSale : x);
    toast.success(resp.message);
    clearEdit();
  })
}

fetchAllCustomers("", (resp) => {
  if(resp.status != 200) {
    toast.error(resp.message);
    return;
  }
  customers.value = resp.data!;
});

fetchAllProducts("", (resp) => {
  if(resp.status != 200) {
    toast.error(resp.message);
    return;
  }
  products.value = resp.data!;
});

submit(true);

</script>

<template>

  <div v-if="waitingRemoval != undefined" class="popup">
    <p>Você tem certeza que deseja deletar a venda Nº<b>{{ waitingRemoval.id }}?</b> Por consequência, isso irá deletar todas as vendas associadas ao mesmo.</p>
    <div class="mx-auto gap-x-2 flex mt-4 justify-center">
      <button @click="remove" class="confirm-button">Confirmar</button>
      <button @click="waitingRemoval = undefined" class="cancel-button">Cancelar</button>
    </div>
  </div>

  <h1 class="text-2xl font-bold text-indigo-600">Listar Produtos</h1>
  <div class="mt-4">
    <div class="flex gap-x-2">
      <p class="text-sm font-light my-auto">Filtrar por descrição:</p>
      <input type="text" v-model="descFilter" :tabindex="0"/>
      <p class="text-sm font-light my-auto">Filtrar por cliente:</p>
      <input type="text" v-model="custFilter" :tabindex="0"/>
      <button class="c-button" :tabindex="1" @click="submit(true)">Buscar</button>
    </div>
    <div class="mt-4">
      <table class="w-full border-[1px] border-indigo-300">
        <tr>
          <th class="w-[5%] text-left">ID</th>
          <th class="w-[20%] text-left">Cliente</th>
          <th class="w-[20%] text-left">Produto</th>
          <th class="w-[7%] text-left">Quantidade</th>
          <th class="w-[10%] text-left">Valor Unitário</th>
          <th class="w-[10%] text-left">Total</th>
          <th class="w-[18%] text-left">Data</th>
          <th class="w-[10%] text-left">Ações</th>
        </tr>
        <tr v-for="(sale, i) in currentPage" class="border-b-[1px] border-indigo-300" :class="[i % 2 != 0 ? 'bg-indigo-50' : 'bg-white']">
          <td class="w-[5%]">{{ sale.id }}</td>
          <td class="w-[20%]">
            <select class="w-full" v-if="editingSale?.id == sale.id" v-model="editingCustomer">
              <option v-for="cust in customers" :value="cust" class="w-full" > {{ cust.name }} </option>
            </select>
            <p v-else>{{ sale.customer.name }}</p>
          </td>
          <td class="w-[20%]">
            <select v-if="editingSale?.id == sale.id" v-model="editingProduct">
              <option v-for="pr in products" :value="pr" class="w-full" > {{ pr.description }} </option>
            </select>
            <p v-else>{{ sale.product.description }}</p>
          </td>
          <td class="w-[10%]">
            <input class="w-full" v-if="editingSale?.id == sale.id" type="number" v-model="editingQuantity"/>
            <p v-else>{{ sale.quantity }}</p>
          </td>
          <td class="w-[10%]">{{ formatCurrency(sale.product.unitaryPrice) }}</td>
          <td class="w-[10%]">{{ formatCurrency(sale.total) }}</td>
          <td class="w-[15%]">
            <input v-if="editingSale?.id == sale.id" type="datetime-local" class="w-full" v-model="editingDate"/>
            <p v-else>{{ new Date(sale.date).toLocaleString() }}</p>
          </td>
          <td class="w-[10%]">
            <div v-if="editingSale?.id != sale.id" class="flex gap-x-2">
              <img src="/edit-svgrepo-com.svg" class="action-button border-indigo-300 bg-indigo-50" @click="startEditing(sale)"/>
              <img src="/bin-svgrepo-com.svg" class="action-button border-red-300 bg-red-50" @click="waitingRemoval = sale"/>
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