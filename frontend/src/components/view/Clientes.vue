<script setup lang="ts">
import { ref } from 'vue';
import CreateCliente from './clientes/CreateCliente.vue';
import ListClientes from './clientes/ListClientes.vue';


const option = ref(0)

</script>

<template>
  <div class="w-screen h-screen p-16 bg-indigo-100">
    <div class="w-full rounded-lg">
      <div class="tabs-header">
        <div :class="[option == 0 ? 'bg-indigo-600' : 'bg-indigo-400']" class="tab" @click="option = 0">
          <p>Novo Cliente</p>
        </div>
        <div :class="[option == 1 ? 'bg-indigo-600' : 'bg-indigo-400']" class="tab" @click="option = 1">
          <p>Listar Clientes</p>
        </div>
      </div>
      <div class="w-full h-full p-8 bg-white border-[1px] border-indigo-400 rounded-lg mt-2 shadow-lg">
        <CreateCliente v-if="option == 0"/>
        <ListClientes v-if="option == 1"/>
      </div>
    </div>

  </div>
</template>