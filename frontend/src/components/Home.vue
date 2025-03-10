<script setup lang="ts">
import { useRouter } from 'vue-router';
import { redirectAndRefresh } from '../utils';
import { useToast } from 'vue-toastification';
import { ref } from 'vue';
import { dataLoad } from '../api/data_load';

const router = useRouter();
const toast = useToast();

const waitingConfirm = ref(false);

function startDataLoad() {
  if(!waitingConfirm.value) {
    waitingConfirm.value = true;
    return;
  }
  waitingConfirm.value = false;
  dataLoad((r) => {
    if(r.status != 200) {
      toast.error(r.message);
      return;
    }
    toast.success(r.message);
  })
}

</script>

<template>

  <div v-if="waitingConfirm" class="popup">
    <p>Você tem certeza que deseja importar os dados da Campos Dealer? Isso irá limpar todas as tabelas antes de importar.</p>
    <div class="mx-auto gap-x-2 flex mt-4 justify-center">
      <button @click="startDataLoad" class="confirm-button">Confirmar</button>
      <button @click="waitingConfirm = false" class="cancel-button">Cancelar</button>
    </div>
  </div>

  <div class="w-screen h-screen flex flex-col gap-y-4 justify-center items-center">
    <button @click="startDataLoad" class="w-64 gap-x-1 text-center justify-center text-white py-4 font-bold rounded-lg bg-blue-500 flex">
      <img src="/import-svgrepo-com.svg" class="w-8">
      <p class="my-auto">Importar dados</p>
    </button>
    <button @click="redirectAndRefresh('/produtos', router)" class="w-64 gap-x-1 text-center justify-center text-white py-4 font-bold rounded-lg bg-pink-500 flex">
      <img src="/package-box-ui-4-svgrepo-com.svg" class="w-8">
      <p class="my-auto">Gerenciar Produtos</p>
    </button>
    <button @click="redirectAndRefresh('/clientes', router)" class="w-64 gap-x-1 text-center justify-center text-white py-4 font-bold rounded-lg bg-amber-500 flex">
      <img src="/user-svgrepo-com.svg" class="w-8">
      <p class="my-auto">Gerenciar Clientes</p>
    </button>
    <button @click="redirectAndRefresh('/vendas', router)" class="w-64 gap-x-1 text-center justify-center text-white py-4 font-bold rounded-lg bg-green-500 flex">
      <img src="/bag-shopping-svgrepo-com.svg" class="w-8">
      <p class="my-auto">Gerenciar Vendas</p>
    </button>
  </div>
</template>