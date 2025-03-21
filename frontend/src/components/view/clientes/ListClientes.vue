<script setup lang="ts">
import { ref } from 'vue';
import type Cliente from '../../../model/Cliente';
import { deleteCustomer, fetchCustomers, updateCustomer } from '../../../api/clientes';
import { useToast } from 'vue-toastification';

const toast = useToast();

const ufs = ref([]);
const counties = ref([]);
const filter = ref("");
const lastFilter = ref("");

const previousPage = ref<Cliente[]>([]);
const currentPage = ref<Cliente[]>([]);
const nextPage = ref<Cliente[]>([]);

const page = ref(1);

const editingCust = ref<Cliente | undefined>(undefined);
const editingName = ref("");
const editingUF = ref("");
const editingCounty = ref("");

const waitingRemoval = ref<Cliente | undefined>(undefined) 

fetch("https://servicodados.ibge.gov.br/api/v1/localidades/estados")
  .then(response => response.json())
  .then(json => ufs.value = json.map((estado: any) => estado.sigla));
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

function fetchPage(offset: number, then: (v: Cliente[]) => void) {
  if(lastFilter.value != filter.value) {
    page.value = 1;
    lastFilter.value = filter.value;
  }
  fetchCustomers(page.value + offset, filter.value, (c) => {
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
  deleteCustomer(waitingRemoval.value!.id, (resp) => {
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

function startEditing(c: Cliente) {
  editingCust.value = c;
  editingName.value = c.name;
  editingUF.value = c.county.split("/")[1];
  onEstadoChanged();
  editingCounty.value = c.county.split("/")[0];
}

function clearEdit() {
  editingCust.value = undefined;
  editingName.value = "";
  editingUF.value = "";
  editingCounty.value = "";
  counties.value = [];
}

function saveEditing() {
  let newCidade = editingCounty.value + "/" + editingUF.value;
  updateCustomer(({id: editingCust.value!.id, county: newCidade, name: editingName.value}), (resp) => {
    if(resp.status != 200) {
      resp.errors.forEach(x => toast.error(x));
      toast.error(resp.message);
      return;
    }
    currentPage.value = currentPage.value.map(x => x.id == editingCust.value?.id ? { id: x.id, county: newCidade, name: editingName.value } : x);
    toast.success(resp.message);
    clearEdit();
  })
}

function onEstadoChanged() {
  editingCounty.value = "";
  fetch(`https://servicodados.ibge.gov.br/api/v1/localidades/estados/${editingUF.value}/municipios`)
    .then(response => response.json())
    .then(json => counties.value = json.map((municipio: any) => municipio.nome));
}

submit(true);

</script>

<template>

  <div v-if="waitingRemoval != undefined" class="popup">
    <p>Você tem certeza que deseja deletar o cliente <b>{{ waitingRemoval.name }}?</b> Por consequência, isso irá deletar todas as vendas associadas ao mesmo.</p>
    <div class="mx-auto gap-x-2 flex mt-4 justify-center">
      <button @click="remove" class="confirm-button">Confirmar</button>
      <button @click="waitingRemoval = undefined" class="cancel-button">Cancelar</button>
    </div>
  </div>

  <h1 class="text-2xl font-bold text-indigo-600">Listar Clientes</h1>
  <div class="mt-4">
    <div class="flex gap-x-2">
      <p class="text-sm font-light my-auto">Filtrar por nome:</p>
      <input type="text" v-model="filter" :tabindex="0"/>
      <button class="c-button" :tabindex="1" @click="submit(true)">Buscar</button>
    </div>
    <div class="mt-4">
      <table class="w-full border-[1px] border-indigo-300">
        <tr>
          <th class="w-[10%] text-left">ID</th>
          <th class="w-[35%] text-left">Nome</th>
          <th class="w-[35%] text-left">Cidade</th>
          <th class="w-[20%] text-left">Ações</th>
        </tr>
        <tr v-for="(cliente, i) in currentPage" class="border-b-[1px] border-indigo-300" :class="[i % 2 != 0 ? 'bg-indigo-50' : 'bg-white']">
          <td class="w-[10%]">{{ cliente.id }}</td>
          <td class="w-[35%]">

            <input v-if="editingCust?.id == cliente.id" v-model="editingName" type="text" class="w-2/3"/>
            <p v-else>{{ cliente.name }}</p>

          </td>
          <td class="w-[35%]">
            <div v-if="editingCust?.id == cliente.id">

              <label class="ml-1 text-xs">UF</label>

              <select v-model="editingUF" @change="onEstadoChanged" class="bg-white w-16 text-xs">
                <option v-for="opt in ufs" :value="opt">{{ opt }}</option>
              </select>

              <label class="ml-1 text-xs">Cidade</label>

              <select v-model="editingCounty" :disabled="editingUF == ''" :class="[editingUF == '' ? 'bg-gray-100' : 'bg-white']" class="text-xs">
                <option v-for="opt in counties" :value="opt">{{ opt }}</option>
              </select>

            </div>
            <p v-else class="text-sm"> {{ cliente.county }} </p>
          </td>
          <td class="w-[20%]">

            <div v-if="editingCust?.id != cliente.id" class="flex gap-x-2">
              <img src="/edit-svgrepo-com.svg" class="action-button border-indigo-300 bg-indigo-50" @click="startEditing(cliente)"/>
              <img src="/bin-svgrepo-com.svg" class="action-button border-red-300 bg-red-50" @click="waitingRemoval = cliente"/>
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