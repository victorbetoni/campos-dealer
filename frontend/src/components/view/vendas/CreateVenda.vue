<script lang="ts" setup>
import { ref } from 'vue';
import { useToast } from 'vue-toastification';
import { createSale } from '../../../api/vendas';
import type Cliente from '../../../model/Cliente';
import type Produto from '../../../model/Produto';
import { fetchAllCustomers } from '../../../api/clientes';
import { fetchAllProducts } from '../../../api/produtos';
import { formatCurrency } from '../../../utils';

const toast = useToast();

const customers = ref<Cliente[]>([]);
const products = ref<Produto[]>([]);

const product = ref<Produto | undefined>(undefined);
const customerId = ref(-1);
const quantity = ref(0);

function submit() {
  if(product == undefined || customerId.value < 0 || quantity.value < 0) {
    toast.error("Preencha todos os campos.");
    return;
  }
  createSale(product.value!.id, customerId.value, quantity.value | 0, (resp) => {
    if(resp.status != 200) {
      toast.error(resp.message);
      return;
    }
    toast.success("Venda criada com sucesso.");
    product.value = undefined;
    customerId.value = -1;
    quantity.value = 1;
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

</script>

<template>
  <h1 class="text-2xl font-bold text-indigo-600">Criar Venda</h1>
  <div>
    <div>
      <p class="text-sm mt-2">Cliente</p>
      <select v-model="customerId">
        <option v-for="cliente in customers" :value="cliente.id" > {{ cliente.name }} </option>
      </select>
    </div>
    <div>
      <p class="text-sm mt-2">Produto</p>
      <select v-model="product">
        <option v-for="produto in products" :value="produto" > {{ produto.description }} </option>
      </select>
    </div>
    <div>
      <p class="text-sm mt-2">Quantidade</p>
      <input type="number" v-model="quantity"/>
    </div>
    <p class="mt-2">Total da venda: <b>{{formatCurrency(((product == undefined ? 0 : product.unitaryPrice) * quantity))}}</b></p>
    <button class="c-button mt-4" @click="submit">Enviar</button>
  </div>
</template>