import { createApp } from 'vue'
import './style.css'
import money from 'v-money3'
import App from './App.vue'
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";

createApp(App).use(Toast, {}).use(money).mount('#app')
