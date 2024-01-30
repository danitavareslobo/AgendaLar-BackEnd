<h1 align="center"> Agenda Lar - BackEnd </h1>

<br>

Esse projeto faz parte da avaliação do candidato ao processo seletivo da [**Cooperativa Lar**](https://www.lar.ind.br). Nesse repositório está a aplicação BackEnd do software, será possível cadastrar, listar, editar e deletar pessoas, e seus telefones para contato.

<br>

## Tecnologias utilizadas

`C#` `.NET` `SQL Server` `JWT` `ASP.NET Core Identity`

<br>

Quando recebi a proposta de desenvolver uma WebAPI REST com .NET Core, pensei logo em usar meus conhecimentos com autenticação e autorização combinando Json Web Token com o ASP.NET Core Identity. Usando essas duas ferramentas temos um sistema de controle de usuário robusto, completo e seguro. Em relação a arquitetura, me baseei na Clean architecture, porém utilizando a Controller como mediador para um desenvolvimento mais rápido devido ao tempo disponibilizado, ao invés da tradicional implementação com mediator e seus Handlers. Também procurei manter um domínio rico, seguindo o design do DDD. Utilizei o Entity Framework Core para mapear meu modelo relacional e usei a abordagem CodeFirst (onde escrevemos a classe e a partir das classes gera o banco de dados). Fiquei empolgada em usar o que tem de mais moderno no desenvolvimento .NET. Fiz a documentação da minha API utilizando o Swashbuckle Swagger que é configurado a partir de um método de extensão. 

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/3654e555-b10f-4306-b748-2d21924f64b7)

Neste método configurei também um recurso para poder informar o JWT nos requests que requerem autenticação e autorização. Tenho experiência em trabalhar com JWT com e sem o Identity.  
No registro do Usuário, apesar de não precisar, decidi estender o IdentityUser, mostrando que é possível adicionar quaisquer propriedades que precisarmos num modelo relacional de usuários.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/d6420c71-9525-49e1-bd66-a2a214a9164b)

Na criação dos meus modelos usei o princípio da abstração da programação orientada a objetos para reaproveitar propriedades que serão comuns a todas as entidades do domínio. Criei a classe abstrata Entity, que tem o Id, a data de criação, data de atualização, uma flag para indicar se o registro está ativo, e uma flag para fazer a exclusão lógica do registro (IsDeleted).

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/b632b35e-a1f3-4002-a9b1-9278533ddf79)

Na classe Entity também defini que toda entidade do domínio é responsável por validar a si mesma. Criei uma validação utilizando o ValidationResult da biblioteca FluentValidation. Gosto de utilizar o FluentValidation porque consigo abstrair as validações da entidade sem que a mesma perca a responsabilidade por sua validação.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/958c199e-946f-4815-afa9-f93e16b5ae8c)

Nas minhas entidades gosto de definir os valores máximo e mínimo de cada campo, em constantes para que possa ser reutilizado em diversos pontos da aplicação e caso seja necessário fazer alguma alteração, alteraremos apenas um campo e isso se refletirá para toda a aplicação. Também é uma boa prática para evitar o Hard Coded.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/af1135b2-c674-437d-89c9-86ce4f6c77f8)

Criei a classe Notification para utilizar o conceito/padrão de Projeto Domain Notification Pattern para substituir Exceptions a nível de domínio na aplicação e para reduzir a quantidade de IFs (Complexidade) utilizando uma abordagem por contratos. Gosto dessa abordagem, também, porque ao invés de retornar um erro de cada vez, eu retorno uma lista de possíveis problemas no request, o que no longo prazo pode impactar positivamente no número de requests que a API vai receber. 

Para utilizar esse padrão, implementei o NotificationService. Ele é passado pelos serviços por Injeção de Dependência com ciclo de vida Scopped. Porque dentro do mesmo request ele pode acumular erros de diversas camadas diferentes e esses erros só fazem sentido no contexto do mesmo request. Assim, quando o request é finalizado eu verifico se existem notificações e se existirem retorno a notificação com o seu devido status code correspondente. 

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/e7b1307d-e193-4ff8-9589-ea1c50a65e17)

Para trabalhar em conjunto com o NotificationService criei uma Controller base com o nome de DefaultController, ela interage com o NotificationService verificando e formatando suas notificações sempre que as classes filhas utilizam seu método de CustomResponse. 

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/81937558-9075-4897-9e11-bc5609d1792b)


![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/56c09b74-2690-4314-aab1-03ef2c0ad9b3)

Ainda na DefaultController, implementei uma propriedade protegida responsável por obter o Id do Usuário que está realizando o request.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/4ac37ff3-c09a-47c6-8bdb-7c6c9dbcc201)

O usuário é definido a partir do Token JWT informado no Header do request. Esse Token é obtido pela aplicação automaticamente graças ao Identity e fica disponível no HttpContext.

As minhas Controllers são decoradas com o atributo de Autorizer, esse atributo é responsável por verificar a parte de autenticação e autorização. Todos os métodos dessa API precisam de autenticação, exceto Login e Register. A Controller usa o AutoMapper para transformar a ViewModel numa entidade de domínio. A configuração do AutoMapper é feita pelo método de extensão AddAutoMapperConfiguration. Ele busca através do Reflection todas as classes que herdam de profile contidas no mesmo Assembly.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/9ae01d39-ff70-4896-86be-517935d40c77)

Dessa forma, não se faz necessário adicionar mapeamento por mapeamento.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/4198ca89-8196-45c5-bb68-f864d58ce795)

Falando em configuração e reflection, vamos falar sobre o mapeamento do banco de dados. Sua configuração também é realizada utilizando reflection, porém de forma automática graças ao Entity Framework Core.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/7e65dbba-567c-456c-ad87-d6e9c1920e36)

Assim precisamos apenas criar as classe de configuração herdando de IEntityTypeConfiguration baseado na nossa entidade de domínio.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/b06b7e4c-846f-4005-b54c-36f13f1ee090)

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/fb64be30-b24e-4e5d-9e8a-889af12e76aa)

Meus repositórios são baseados na interface IRepository onde pendei no inicio do projeto e quais métodos poderiam ser utilizados pelo Front-End quando o Back-End estivesse pronto. Concebi que seriam necessários alguns métodos básicos como as operações de CRUD e uma listagem paginada.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/495a31a7-747d-4c1a-bb99-33cb732fd56b)

Implementei o IDisposable para que os métodos eliminem da memória tudo que possam utilizar, auxiliando assim o trabalho do Garbage Collector, o que também é bem vindo no que diz respeito a performance e memrória.

Caso seja necessário adicionar algum comportamento além dos métodos padrões, é possível faze-lo diretamente na interface especializada.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/bbf44a45-253d-4e35-9b13-a0db32bc1c44)

Criei alguns métodos de extensão para auxiliar nas validações e outras funções.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/618854a9-e711-411f-b72b-84e689e46da1)

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/ce9a044f-9f61-4996-89be-0ea180135081)

Tenho um filtro que trabalha como um middleware atuando ao final de cada request, para evitar que um notificação seja perdida caso a controller não utilize o método CustomResponse da DefaultController.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/4d6d69e4-d1eb-40f0-b7a0-f0843c919f20)

Por ultimo e não menos importante, chegamos aos serviços. Eles usufruem de toda arquitetura que motamos ao longo do projeto. Obtendo o objeto já mapeado para entidade são responsáveis por realizar a validação de domínio e retornar caso haja algum problema na requisição.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/cd43a871-38d7-401d-ae61-f48095622822)

Também realizamos as verificações de negócio, como  nesse exemplo onde só vou conseguir excluir (de forma lógica) um registro que exista no banco de dados.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/c43bbe76-0385-42a3-bb26-49e11afed50d)

Sobre a exclusão lógica, ela é realizada em duas etapas. A primeira é ao definir o objeto de domínio como "deletado", neste momento conseguimos a partir de qualquer camada saber que o objeto tem uma marcação para que não seja utilizado de forma indevida. A segunda partefica nas consultas, é cruscial que as consultas sejam configurada para retornar apenas os registro que não estão marcados como excluídos. Desta forma temos o melhor dos dois mundos, o usuário tem a percepção da exclusão e a TI mantemos o registro para controle.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/461eae09-ef06-40c2-b9ef-0c75299631b6)

Utilizei o App Secret para para guardar os segredos da minha aplicação como ConnectionString e Secret Key.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/2ef66533-dddf-4b11-8062-e6aa30b24363)

É uma funcionalidade bem legal onde o arquivo fica armazenado na pasta local do desenvolvedor e não sobe juntos com as alterações no repositório remoto. Desta forma podemos utilizar a configuração padrão da microsoft através do IConfiguration.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/0d12ec35-a0f5-4911-81fb-ac1f17fcafb4)

Quando a aplicação for publicada no servidor é necessário que seja configurado no arquivo appsettings.json para que a aplicação consiga realizar as mesmas configurações.

<br>

## Métodos
As reequisições para a API seguem os seguintes padrões:
| Método | Descrição |
|---|---|
| `GET` | Retorna informações de um ou mais registros. |
| `POST` | Utilizado para criar um novo registro. |
| `PUT` | Atualiza dados de um registro ou altera sua situação. |
| `DELETE` | Remove um registro do sistema. |

<br>

## Projeto Publicado / Repositórios

> Vc pode acessar a API BackEnd Publicada clicando aqui: :point_right: https://www.agenda-back.danitavares.dev
>
> <br>
>
> Projeto FrontEnd Publicado: :point_right: https://www.agenda-front.danitavares.dev
>
> Repositório do Projeto FrontEnd: https://github.com/danitavareslobo/AgendaLar-FrontEnd

<br>

## Demonstração do Projeto
Esta API gerência os dados presentes no banco de dados agendalarweb.
No banco de dados estão presentes três tabelas que são: Users, People e Phones. Modeladas como na imagem a seguir:
<br>

![agendalarweb](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/435774d3-9c1e-4da0-afbc-f0aa56b332df)


<br>

https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/01d2a3d3-b181-44e1-af7e-5cbe26108382


<br>

## Acesso ao Projeto
Para utilizar a aplicação, siga as instruções abaixo:


#### Pré-requisitos

É indicado ter instalado em seu computador [VS 2019](https://visualstudio.microsoft.com/pt-br/) ou superior.

Antes de prosseguir, verifique se o seguinte software está instalado em seu computador: - .NET 8 SDK.
<br>


#### Instruções

- Clone o repositório para o seu computador;
- Restaure as dependências do projeto: 
```sh 
dotnet restore
```
- Compile o projeto:
```sh 
dotnet build
```
É necessário configurar a conexão com o banco de dados.
  Abra o arquivo appsettings.json localizado no diretório raiz do projeto e atualize a string de conexão `ServerConnection` do banco de dados com as informações do seu SQL Server.
- Execute as migrações do banco de dados para criar as tabelas necessárias, mais informações [aqui](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli);
- A aplicação está pronta para execução
```sh 
dotnet run
```
<br>

Para acessar e fazer os testes no Swagger, utilize o email: `user1@email.com` e a senha: `User@123` ou cadastre um novo usuário. Após isso copie o `accessToken` sem as aspas, clique em `Authorize` e no campo `Value` Digite Bearer dê um espaço e cole o `accessToken`. Assim terá acesso para executar todas as opções disponíveis.

<br>

## Autoria do Projeto

<div>

> 
> <a href="https://github.com/danitavareslobo"><img src="https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/0ef94e0e-6396-48bb-b331-5ab0916592d8" width= 120 target="_blank"></a>
<a href="https://github.com/danitavareslobo"> <p >  Daniele Tavares Lobo </p></a>

</div>
