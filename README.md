# Victor Hugo Betoni - Teste Técnico Campos Dealer
- Tecnologias utilizadas: Git, TypeScript, Vue.js, Vue Router, TailwindCSS, Vite, .NET e Entity Framework
- Padrão de projeto: MVC com repositórios e pattern de Usecases para separar a camada de negócio.

### Instalação:
- Backend: abrir a solução no Visual Studio e rodar na configuração HTTP. A API ficará disponível em http://localhost:5141 caso a porta esteja disponível. Além disso, talvez seja necessário trocar a propriedade DefaultConnection no appsettings.json dependendo de seu ambiente.
- Frontend: Dentro da pasta do frontend, executar: `npm i` e `npm run dev`. O frontend ficará disponível em http://localhost:5173 caso a porta esteja disponível.  


### Pontuações técnicas:
- Como não havia tempo o suficiente e não estava no escopo do projeto, não foi implementada as funcionalidades de autenticação e autorização, apesar de ser algo indispensável em uma API desse tipo em produção.
-  Os endpoints para a importação de dados aparentam ficar fora do ar em alguns momentos do dia, principalmente a noite. A funcionalidade de importação de dados não funcionará caso o endpoint não esteja disponível no momento que esse programa for testado.

### Pontuações gerais:
- No caso de uso de criação de venda, optei por buscar o valor unitário diretamente da tabela de produtos ao invés de permitir que ele seja enviado pelo frontend.

### Features adicionais:
- Paginação em todos os endpoints de listagem.
- Formatação de moeda onde é necessário

### Melhorias que eu faria com mais tempo:
- Autenticação e autorização
- Middleware para fazer validação de dados
- Caching no frontend e backend com alguma tecnologia apropriada para economizar tempo de busca
- Responsividade no frontend