using System;
using System.Collections.Generic;
using System.Linq;
using EfCore.ConsoleApp.Domain;
using EfCore.ConsoleApp.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EfCore.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // DeleteData();
            // UpdateData();
            // QueryDataLazyLoad();
            // AddOrder();
            // QueryData();
            // InsertBulkData();
            // InsertData();
        }

        private static void DeleteData()
        {
            using var db = new Data.ApplicationContext();

            // var cliente = db.Clientes.Find(6);
            var cliente = new Cliente
            {
                Id = 6
            };

            // db.Clientes.Remove(cliente);
            // db.Remove(cliente);

            db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }

        private static void UpdateData()
        {
            using var db = new Data.ApplicationContext();

            // var cliente = db.Clientes.Find(1);
            // cliente.Nome = "New name 2";

            // db.Clientes.Update(cliente); // UPDATE ALL COLUMNS - NOT A GOOD APPROACH

            var cliente = new Cliente
            {
                Id = 1
            };

            var disconnectedCustomer = new
            {
                Nome = "My new name - disconnected 02",
                Telefone = "7966669998"
            };
            
            db.Attach(cliente);
            db.Entry(cliente).CurrentValues.SetValues(disconnectedCustomer);

            db.SaveChanges();
        }

        private static void QueryDataLazyLoad()
        {
            using var db = new Data.ApplicationContext();

            var pedidos = db.Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .ToList();

            Console.WriteLine($"Amount of Orders: {pedidos.Count}");
        }

        private static void AddOrder()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClientId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Test order",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }

        private static void QueryData()
        {
            using var db = new Data.ApplicationContext();
            // var sintaxQuery = (from c in db.Clientes where c.Id > 0 select c).ToList();
            var methodQuery = db.Clientes
                                .Where(c => c.Id > 0)
                                // .AsNoTracking()
                                .ToList();

            foreach (var customer in methodQuery)
            {
                Console.WriteLine($"Querying customer: {customer.Id}");
                // db.Clientes.Find(customer.Id);
                db.Clientes.FirstOrDefault(c => c.Id == customer.Id);
            }
        }

        private static void InsertBulkData()
        {
            var produto = new  Produto
            {
                Descricao = "Test Product",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaVenda,
                Ativo = true
            };

            var clientes = new List<Cliente>(){
                new Cliente
                {
                    Nome = "Raphael Socolowski",
                    CEP = "99999000",
                    Cidade = "Dublin",
                    Estado = "DB",
                    Telefone = "99000001111"
                },
                new Cliente
                {
                    Nome = "Raphael Lima",
                    CEP = "99999000",
                    Cidade = "Dublin",
                    Estado = "DB",
                    Telefone = "99000001112"
                },
                new Cliente
                {
                    Nome = "Client Test",
                    CEP = "99999005",
                    Cidade = "Dublin",
                    Estado = "DB",
                    Telefone = "99000001113"
                }
            };
            
            using (var db = new Data.ApplicationContext())
            {
                // Memoria tracking
                // db.AddRange(produto, clientes);                
                db.Clientes.AddRange(clientes);

                var records = db.SaveChanges();
                Console.WriteLine($"Total Record(s): {records}");
            }
        }

        private static void InsertData()
        {
            var produto = new  Produto
            {
                Descricao = "Test Product",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaVenda,
                Ativo = true
            };

            using (var db = new Data.ApplicationContext())
            {
                // Memoria tracking
                db.Produtos.Add(produto);
                db.Set<Produto>().Add(produto);
                db.Entry(produto).State = EntityState.Added;
                db.Add(produto);

                var records = db.SaveChanges();
                Console.WriteLine($"Total Record(s): {records}");
            }
        }
    }
}
