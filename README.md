<div align="center">
<img src="https://github.com/user-attachments/assets/208a0ebb-ca7c-4b0b-9f68-0b35050a9880" width="30%" />
</div>

# Lanchonete do Bairro - Pagamentos (POS TECH: TECH CHALLENGE - 4a FASE)🚀

Seja bem vindo! Este é um desafio proposto pela PósTech (Fiap + Alura) na terceira fase da pós graduação de Software Architecture (8SOAT).

📼 Vídeo de demonstração do projeto desta fase: em produção

Integrantes do grupo:<br>
Alexis Cesar (RM 356558)<br>
Bruna Gonçalves (RM 356557)

Esta é uma API em .NET para gerenciar o pagamento de pedidos, utilizando MongoDB como banco de dados e com integração ao Mercado Pago.

A aplicação é containerizada utilizando Docker, orquestrada por Kubernetes (K8s) para garantir escalabilidade e resiliência, e gerenciada por Helm, que automatiza o deployment e rollbacks no cluster Kubernetes (EKS) na nuvem da AWS.

ℹ️ Este repositório faz parte de um conjunto de repositórios (outros serviços, infraestrutura e banco de dados) que formam um sistema de lanchonete. Link de todos os repositórios envolvidos:
- [Infraestrutura AWS](https://github.com/BrunaPisera/postech-tc-infraestrutura)
- [Serviço de Pedidos](https://github.com/BrunaPisera/postech-tc-pedidos)
- [Serviço de Acompanhamento](https://github.com/BrunaPisera/postech-tc-acompanhamento)
- [Serviço de Pagamentos](https://github.com/BrunaPisera/postech-tc-pagamentos)
- [Banco de Dados](https://github.com/BrunaPisera/postech-tc-dbs)
- [Função AWS Lambda](https://github.com/BrunaPisera/postech-tc-lambda)

## Navegação
- [Arquitetura](#arquitetura)
- [Funcionalidades](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)

## Arquitetura

A aplicação segue a Arquitetura Limpa, que promove a separação de responsabilidade, facilitando a manutenção e escalabilidade. Esta abordagem permite que a lógica de negócios principal seja independente de qualquer dependência externa, como bancos de dados ou serviços externos.

## Funcionalidades
 
- **Gerenciamento de Pedidos**: Os clientes podem fazer pedidos via o totem da lanchonete.
- **Gerenciamento de Produtos**: Gerencia os produtos da lanchonete dentro de quatro categorias (Lanche, Bebida, Acompanhamento e Sobremesa).
- **Gerenciamento de Clientes**: Cadastra novos clientes no sistema e verifica existência por CPF.

## Tecnologias Utilizadas
 
- **.NET 8.0**: ASP.NET Core.
- **Entity Framework Core**: ORM para interações com o banco de dados.
- **MongoDB Atlas**: Banco de dados.
- **Docker**: Plataforma de containerização.
- **Kubernetes**: Orquestração de contêineres.
- **Helm**: Gerenciamento de de pacotes kubernetes.
- **EKS**: Cluster Kubernetes na nuvem da AWS.
- **Terraform**: Automação de criação de recursos em provedores de nuvem.
