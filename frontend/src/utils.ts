import type { Router } from "vue-router";

export const DEFAULT_CURRENCY_PROPS = {
  prefix: 'R$ ',
  suffix: '',
  thousands: '.',
  decimal: ',',
  number: true,
  precision: 2,
  disableNegative: false,
  disabled: false,
  min: null,
  max: null,
  allowBlank: false,
  minimumNumberOfCharacters: 0,
  shouldRound: true,
  focusOnRight: false,
};

export const DATE_FORMAT_PROPS = {
  day: '2-digit',
  month: '2-digit',
  year: 'numeric',
  hour: '2-digit',
  minute: '2-digit',
  hour12: false, 
}

export function formatCurrency(price: number): string {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(price);
}

export async function redirectAndRefresh(path: string, router: Router) {
  await router.push(path);
  router.go(0);
}