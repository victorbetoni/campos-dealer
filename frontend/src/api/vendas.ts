import type Venda from "../model/Venda";
import { APIRequest, DELETE, GET, POST, PUT, type Handler } from "./api";

const VENDAS_CONTROLLER = "venda"

export function createSale(productId: number, customerId: number, quantity: number, handler: Handler<Venda>) {
  POST(new APIRequest(VENDAS_CONTROLLER, null, ({
    productId: productId,
    customerId: customerId,
    quantity: quantity
  }), null), handler)
}

export function fetchSales(page: number, name: string, description: string, handler: Handler<Venda[]>) {
  GET(new APIRequest(VENDAS_CONTROLLER, null, null, ({
    desc: description,
    name: name,
    page: page
  })), handler)
}

export function updateSale(c: Venda, handler: Handler<Venda>) {
  PUT(new APIRequest(VENDAS_CONTROLLER, null, {
    id: c.id,
    customerId: c.customer.id,
    productId: c.product.id,
    quantity: c.quantity,
    total: 0,
    date: new Date(c.date).getTime(),
    unitaryValue: 0,
    customer: c.customer,
    product: c.product
  }, null), handler)
};

export function deleteSale(id: number, handler: Handler<any>) {
  DELETE(new APIRequest(VENDAS_CONTROLLER, null, ({id: id}), null), handler)
}