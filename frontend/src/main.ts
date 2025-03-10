import { createApp } from 'vue'
import './style.css'
import money from 'v-money3'
import App from './App.vue'
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";

import { createMemoryHistory, createRouter, createWebHashHistory, createWebHistory } from 'vue-router'
import Home from './components/Home.vue';
import Vendas from './components/view/Vendas.vue';
import Clientes from './components/view/Clientes.vue';
import Produtos from './components/view/Produtos.vue';

const routes = [
  { path: '/', component: Home },
  { path: '/vendas', component: Vendas },
  { path: '/clientes', component: Clientes },
  { path: '/produtos', component: Produtos },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes,
})

createApp(App).use(router).use(Toast, {}).use(money).mount('#app')
