<script lang="ts" setup>
import { ref } from 'vue';
import { createCustomer } from '../../../api/clientes';
import { useToast } from 'vue-toastification';

const toast = useToast();
const estados = ref([]);
const municipios = ref([]);

const nome = ref("");
const estado = ref("");
const municipio = ref("");

function onEstadoChanged() {
  municipio.value = "";
  fetch(`https://servicodados.ibge.gov.br/api/v1/localidades/estados/${estado.value}/municipios`)
    .then(response => response.json())
    .then(json => municipios.value = json.map((municipio: any) => municipio.nome));
}

function submit() {

  if(municipio.value.trim() == "" || estado.value.trim() == "" || nome.value.trim() == "") {
    toast.error("Preencha todos os campos!");
    return;
  }

  let cidade = municipio.value + "/" + estado.value;
  createCustomer(cidade, nome.value, (resp) => {
    if(resp.status == 400) {
      toast.error("Preencha todos os campos!");
      return;
    }
    if(resp.status != 200) {
      toast.error(resp.message);
      return;
    }
    toast.success("Cliente criado com sucesso.");
    nome.value = "";
    estado.value = "";
    municipio.value = "";
  })
}

fetch("https://servicodados.ibge.gov.br/api/v1/localidades/estados")
  .then(response => response.json())
  .then(json => estados.value = json.map((estado: any) => estado.sigla));

</script>

<template>
  <h1 class="text-2xl font-bold text-indigo-600">Criar Cliente</h1>
  <div>
    <div>
      <p class="text-sm mt-2">Nome</p>
      <input type="text" v-model="nome"/>
    </div>
    <div>
      <p class="text-sm mt-2">Estado</p>
      <select v-model="estado" @change="onEstadoChanged" class="bg-white">
        <option v-for="opt in estados" :value="opt">{{ opt }}</option>
      </select>
    </div>
    <div>
      <p class="text-sm mt-2">Cidade</p>
      <select v-model="municipio" :disabled="estado == ''" :class="[estado == '' ? 'bg-gray-100' : 'bg-white']">
        <option v-for="opt in municipios" :value="opt">{{ opt }}</option>
      </select>
    </div>
    <button class="c-button mt-4" @click="submit">Enviar</button>
  </div>
</template>