<script lang="ts" setup>
import { ref } from 'vue';
import { useToast } from 'vue-toastification';
import { createProduct } from '../../../api/produtos';
import { DEFAULT_CURRENCY_PROPS } from '../../../utils';

const toast = useToast();

const desc = ref("");
const price = ref("");
const currencyProps = ref(DEFAULT_CURRENCY_PROPS);

function submit() {
  const parsedPrice = parseFloat(price.value.replace('R$', '').replace(/\./g, '').replace(',', '.').trim());
  if(parsedPrice < 0 || parsedPrice > 3.40e+38) {
    toast.error("Preço inválido!");
    return;
  }
  if(parsedPrice <= 0 || desc.value.trim() == "") {
    toast.error("Preencha todos os campos!");
    return;
  }

  createProduct(desc.value, parsedPrice, (resp) => {
    if(resp.status != 200) {
      toast.error(resp.message);
      return;
    }
    toast.success("Produto criado com sucesso.");
    desc.value = "";
    price.value = "R$ 0.00";
  })
}
</script>

<template>
  <h1 class="text-2xl font-bold text-indigo-600">Criar Produto</h1>
  <div>
    <div>
      <p class="text-sm mt-2">Descrição</p>
      <input type="text" v-model="desc"/>
    </div>
    <div>
      <p class="text-sm mt-2">Preço</p>
      <input v-model.lazy="price" v-money3="currencyProps"/>
    </div>
    <button class="c-button mt-4" @click="submit">Enviar</button>
  </div>
</template>