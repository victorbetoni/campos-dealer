<script setup lang="ts">
import { ref } from 'vue';
import CreateProduto from './produtos/CreateProduto.vue';
import ListProdutos from './produtos/ListProdutos.vue';
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
          <p>Novo Produto</p>
        </div>
        <div :class="[option == 1 ? 'bg-indigo-600' : 'bg-indigo-400']" class="tab" @click="option = 1">
          <p>Listar Produtos</p>
        </div>
        <div @click="redirectAndRefresh('/', router)" class="tab bg-orange-400 hover:bg-orange-500">
          <p>Voltar</p>
        </div>
      </div>
      <div class="w-full h-full p-8 bg-white border-[1px] border-indigo-400 rounded-lg mt-2 shadow-lg">
        <CreateProduto v-if="option == 0"/>
        <ListProdutos v-if="option == 1"/>
      </div>
    </div>

  </div>
</template>