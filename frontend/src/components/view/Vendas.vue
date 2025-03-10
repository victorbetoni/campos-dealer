<script setup lang="ts">
import { ref } from 'vue';
import CreateVenda from './vendas/CreateVenda.vue';
import ListVendas from './vendas/ListVendas.vue';
import { redirectAndRefresh } from '../../utils';
import { useRouter } from 'vue-router';

const option = ref(0)
const router = useRouter();
</script>

<template>
  <div class="w-screen h-screen p-16 bg-indigo-100">
    <div class="w-full rounded-lg">
      <div class="tabs-header">
        <div :class="[option == 0 ? 'bg-indigo-600' : 'bg-indigo-400']" class="tab" @click="option = 0">
          <p>Nova Venda</p>
        </div>
        <div :class="[option == 1 ? 'bg-indigo-600' : 'bg-indigo-400']" class="tab" @click="option = 1">
          <p>Listar Vendas</p>
        </div>
        <div @click="redirectAndRefresh('/', router)" class="tab bg-orange-400 hover:bg-orange-500">
          <p>Voltar</p>
        </div>
      </div>
      <div class="w-full h-full p-8 bg-white border-[1px] border-indigo-400 rounded-lg mt-2 shadow-lg">
        <CreateVenda v-if="option == 0"/>
        <ListVendas v-if="option == 1"/>
      </div>
    </div>

  </div>
</template>