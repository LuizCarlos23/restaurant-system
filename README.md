# 🍽️ Restaurante MVC

Este é um sistema de restaurante desenvolvido em **ASP.NET Core MVC**. O sistema permite que os usuários realizem pedidos personalizados, adicionando ingredientes opcionais aos alimentos, visualizem o status de seus pedidos e muito mais.

## ✅ Funcionalidades Concluídas

- 👤 **Cadastro e Login de Usuário**: Usuários podem se registrar e acessar o sistema.
- 📋 **Catálogo de Alimentos**: Listagem de alimentos disponíveis para pedido, com opção de personalização.
- 🍕 **Personalização de Pedidos**: Adição de ingredientes extras e ajuste de quantidades.
- 🛒 **Carrinho de Compras**: Resumo dos itens selecionados, quantidade e cálculo do preço total.
- 📦 **Gerenciamento de Pedidos**: Visualização do status dos pedidos realizados.

## 🛠️ Em Desenvolvimento

- 🔐 Regras de acesso diferenciadas para admin e usuário comum
- 🎨 Melhorias no design e responsividade (mobile friendly).
- 🧪 Implementação de testes automatizados (unitários e de integração).

## 💡 Melhorias Futuras

- 🗃️ Histórico detalhado de pedidos com filtros por data/status.
- 📊 Relatórios de vendas e estatísticas para o administrador.
- 🔔 Sistema de notificações para atualização do status dos pedidos.
- 💳 Integração com sistema de pagamento (ex: Pix, Cartão de Crédito).

## 🧰 Tecnologias Utilizadas

- **ASP.NET Core MVC**
- **C#**
- **Entity Framework Core** (para interação com banco de dados)
- **HTML, CSS, JavaScript**
- **Bootstrap e jQuery** (para estilização e interatividade)

## 🚀 Como Rodar o Projeto

1. Clone o repositório:
   ```sh
   git clone https://github.com/LuizCarlos23/restaurant-system.git
   ```
2. Acesse a pasta do projeto:
   ```sh
   cd restaurant-system
   ```
3. Restaure as dependências:
   ```sh
   dotnet restore
   ```
4. Configure o banco de dados (migrations e update):
   ```sh
   dotnet ef database update
   ```
5. Execute o projeto:
   ```sh
   dotnet run
   ```
