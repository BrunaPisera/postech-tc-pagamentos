conn = new Mongo();
db = conn.getDB("PagamentosDB");

db.createCollection("Pagamentos");