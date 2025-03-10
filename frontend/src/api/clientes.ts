import type Cliente from "../model/Cliente";
import { APIRequest, DELETE, GET, POST, PUT, type Handler } from "./api";

const CLIENTE_CONTROLLER = "cliente"

export function createCustomer(county: string, name: string, handler: Handler<Cliente>) {
  POST(new APIRequest(CLIENTE_CONTROLLER, null, ({
    county: county,
    name: name
  }), null), handler)
}

export function fetchCustomers(page: number, name: string, handler: Handler<Cliente[]>) {
  GET(new APIRequest(CLIENTE_CONTROLLER, null, null, ({
    name: name,
    page: page
  })), handler)
}

export function fetchAllCustomers(name: string, handler: Handler<Cliente[]>) {
  fetchCustomers(-1, name, handler);
}

export function updateCustomer(c: Cliente, handler: Handler<Cliente>) {
  PUT(new APIRequest(CLIENTE_CONTROLLER, null, c, null), handler)
};

export function deleteCustomer(id: number, handler: Handler<any>) {
  DELETE(new APIRequest(CLIENTE_CONTROLLER, null, ({id: id}), null), handler)
}