import type Cliente from "../model/Cliente";
import { APIRequest, DELETE, GET, POST, PUT, type Handler } from "./api";

const CLIENTE_CONTROLLER = "Sites/TesteAPI/cliente"

export function createCliente(cidade: string, nome: string, handler: Handler<Cliente>) {
  POST(new APIRequest(CLIENTE_CONTROLLER, null, ({
    county: cidade,
    name: nome
  }), null), handler)
}

export function fetchClientes(page: number, name: string, handler: Handler<Cliente[]>) {
  GET(new APIRequest(CLIENTE_CONTROLLER, null, null, ({
    name: name,
    page: page
  })), handler)
}

export function updateCliente(c: Cliente, handler: Handler<Cliente>) {
  PUT(new APIRequest(CLIENTE_CONTROLLER, null, c, null), handler)
};

export function deleteCliente(id: number, handler: Handler<any>) {
  DELETE(new APIRequest(CLIENTE_CONTROLLER, null, ({id: id}), null), handler)
}