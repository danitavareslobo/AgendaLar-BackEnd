<h1 align="center"> Agenda Lar - BackEnd </h1>

<br>

Esse projeto faz parte da avalia��o do candidato ao processo seletivo da [**Cooperativa Lar**](https://www.lar.ind.br). Nesse reposit�rio est� a aplica��o BackEnd do software, ser� poss�vel cadastrar, listar, editar e deletar pessoas, e seus telefones para contato.

<br>

## Tecnologias utilizadas

`C#` `.NET` `SQL Server` `JWT` `ASP.NET Core Identity`

<br>

Quando recebi a proposta de desenvolver uma WebAPI REST com .NET Core, pensei logo em usar meus conhecimentos com autentica��o e autoriza��o combinando Json Web Token com o ASP.NET Core Identity. Usando essas duas ferramentas temos um sistema de controle de usu�rio robusto, completo e seguro. Em rela��o a arquitetura, me baseei na Clean architecture, por�m utilizando a Controller como mediador para um desenvolvimento mais r�pido devido ao tempo disponibilizado, ao inv�s da tradicional implementa��o com mediator e seus Handlers. Tamb�m procurei manter um dom�nio rico, seguindo o design do DDD. Utilizei o Entity Framework Core para mapear meu modelo relacional e usei a abordagem CodeFirst (onde escrevemos a classe e a partir das classes gera o banco de dados). Fiquei empolgada em usar o que tem de mais moderno no desenvolvimento .NET. Fiz a documenta��o da minha API utilizando o Swashbuckle Swagger que � configurado a partir de um m�todo de extens�o. 

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/3654e555-b10f-4306-b748-2d21924f64b7)

Neste m�todo configurei tamb�m um recurso para poder informar o JWT nos requests que requerem autentica��o e autoriza��o. Tenho experi�ncia em trabalhar com JWT com e sem o Identity.  
No registro do Usu�rio, apesar de n�o precisar, decidi estender o IdentityUser, mostrando que � poss�vel adicionar quaisquer propriedades que precisarmos num modelo relacional de usu�rios.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/d6420c71-9525-49e1-bd66-a2a214a9164b)

Na cria��o dos meus modelos usei o princ�pio da abstra��o da programa��o orientada a objetos para reaproveitar propriedades que ser�o comuns a todas as entidades do dom�nio. Criei a classe abstrata Entity, que tem o Id, a data de cria��o, data de atualiza��o, uma flag para indicar se o registro est� ativo, e uma flag para fazer a exclus�o l�gica do registro (IsDeleted).

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/b632b35e-a1f3-4002-a9b1-9278533ddf79)

Na classe Entity tamb�m defini que toda entidade do dom�nio � respons�vel por validar a si mesma. Criei uma valida��o utilizando o ValidationResult da biblioteca FluentValidation. Gosto de utilizar o FluentValidation porque consigo abstrair as valida��es da entidade sem que a mesma perca a responsabilidade por sua valida��o.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/958c199e-946f-4815-afa9-f93e16b5ae8c)

Nas minhas entidades gosto de definir os valores m�ximo e m�nimo de cada campo, em constantes para que possa ser reutilizado em diversos pontos da aplica��o e caso seja necess�rio fazer alguma altera��o, alteraremos apenas um campo e isso se refletir� para toda a aplica��o. Tamb�m � uma boa pr�tica para evitar o Hard Coded.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/af1135b2-c674-437d-89c9-86ce4f6c77f8)

Criei a classe Notification para utilizar o conceito/padr�o de Projeto Domain Notification Pattern para substituir Exceptions a n�vel de dom�nio na aplica��o e para reduzir a quantidade de IFs (Complexidade) utilizando uma abordagem�por�contratos. Gosto dessa abordagem, tamb�m, porque ao inv�s de retornar um erro de cada vez, eu retorno uma lista de poss�veis problemas no request, o que no longo prazo pode impactar positivamente no n�mero de requests que a API vai receber. 

Para utilizar esse padr�o, implementei o NotificationService. Ele � passado pelos servi�os por Inje��o de Depend�ncia com ciclo de vida Scopped. Porque dentro do mesmo request ele pode acumular erros de diversas camadas diferentes e esses erros s� fazem sentido no contexto do mesmo request. Assim, quando o request � finalizado eu verifico se existem notifica��es e se existirem retorno a notifica��o com o seu devido status code correspondente. 

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/e7b1307d-e193-4ff8-9589-ea1c50a65e17)

Para trabalhar em conjunto com o NotificationService criei uma Controller base com o nome de DefaultController, ela interage com o NotificationService verificando e formatando suas notifica��es sempre que as classes filhas utilizam seu m�todo de CustomResponse. 

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/81937558-9075-4897-9e11-bc5609d1792b)


![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/56c09b74-2690-4314-aab1-03ef2c0ad9b3)

Ainda na DefaultController, implementei uma propriedade protegida respons�vel por obter o Id do Usu�rio que est� realizando o request.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/4ac37ff3-c09a-47c6-8bdb-7c6c9dbcc201)

O usu�rio � definido a partir do Token JWT informado no Header do request. Esse Token � obtido pela aplica��o automaticamente gra�as ao Identity e fica dispon�vel no HttpContext.

As minhas Controllers s�o decoradas com o atributo de Autorizer, esse atributo � respons�vel por verificar a parte de autentica��o e autoriza��o. Todos os m�todos dessa API precisam de autentica��o, exceto Login e Register. A Controller usa o AutoMapper para transformar a ViewModel numa entidade de dom�nio. A configura��o do AutoMapper � feita pelo m�todo de extens�o AddAutoMapperConfiguration. Ele busca atrav�s do Reflection todas as classes que herdam de profile contidas no mesmo Assembly.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/9ae01d39-ff70-4896-86be-517935d40c77)

Dessa forma, n�o se faz necess�rio adicionar mapeamento por mapeamento.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/4198ca89-8196-45c5-bb68-f864d58ce795)

Falando em configura��o e reflection, vamos falar sobre o mapeamento do banco de dados. Sua configura��o tamb�m � realizada utilizando reflection, por�m de forma autom�tica gra�as ao Entity Framework Core.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/7e65dbba-567c-456c-ad87-d6e9c1920e36)

Assim precisamos apenas criar as classe de configura��o herdando de IEntityTypeConfiguration baseado na nossa entidade de dom�nio.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/b06b7e4c-846f-4005-b54c-36f13f1ee090)

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/fb64be30-b24e-4e5d-9e8a-889af12e76aa)

Meus reposit�rios s�o baseados na interface IRepository onde pendei no inicio do projeto e quais m�todos poderiam ser utilizados pelo Front-End quando o Back-End estivesse pronto. Concebi que seriam necess�rios alguns m�todos b�sicos como as opera��es de CRUD e uma listagem paginada.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/495a31a7-747d-4c1a-bb99-33cb732fd56b)

Implementei o IDisposable para que os m�todos eliminem da mem�ria tudo que possam utilizar, auxiliando assim o trabalho do Garbage Collector, o que tamb�m � bem vindo no que diz respeito a performance e memr�ria.

Caso seja necess�rio adicionar algum comportamento al�m dos m�todos padr�es, � poss�vel faze-lo diretamente na interface especializada.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/bbf44a45-253d-4e35-9b13-a0db32bc1c44)

Criei alguns m�todos de extens�o para auxiliar nas valida��es e outras fun��es.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/618854a9-e711-411f-b72b-84e689e46da1)

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/ce9a044f-9f61-4996-89be-0ea180135081)

Tenho um filtro que trabalha como um middleware atuando ao final de cada request, para evitar que um notifica��o seja perdida caso a controller n�o utilize o m�todo CustomResponse da DefaultController.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/4d6d69e4-d1eb-40f0-b7a0-f0843c919f20)

Por ultimo e n�o menos importante, chegamos aos servi�os. Eles usufruem de toda arquitetura que motamos ao longo do projeto. Obtendo o objeto j� mapeado para entidade s�o respons�veis por realizar a valida��o de dom�nio e retornar caso haja algum problema na requisi��o.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/cd43a871-38d7-401d-ae61-f48095622822)

Tamb�m realizamos as verifica��es de neg�cio, como  nesse exemplo onde s� vou conseguir excluir (de forma l�gica) um registro que exista no banco de dados.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/c43bbe76-0385-42a3-bb26-49e11afed50d)

Sobre a exclus�o l�gica, ela � realizada em duas etapas. A primeira � ao definir o objeto de dom�nio como "deletado", neste momento conseguimos a partir de qualquer camada saber que o objeto tem uma marca��o para que n�o seja utilizado de forma indevida. A segunda partefica nas consultas, � cruscial que as consultas sejam configurada para retornar apenas os registro que n�o est�o marcados como exclu�dos. Desta forma temos o melhor dos dois mundos, o usu�rio tem a percep��o da exclus�o e a TI mantemos o registro para controle.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/461eae09-ef06-40c2-b9ef-0c75299631b6)

Utilizei o App Secret para para guardar os segredos da minha aplica��o como ConnectionString e Secret Key.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/2ef66533-dddf-4b11-8062-e6aa30b24363)

� uma funcionalidade bem legal onde o arquivo fica armazenado na pasta local do desenvolvedor e n�o sobe juntos com as altera��es no reposit�rio remoto. Desta forma podemos utilizar a configura��o padr�o da microsoft atrav�s do IConfiguration.

![image](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/0d12ec35-a0f5-4911-81fb-ac1f17fcafb4)

Quando a aplica��o for publicada no servidor � necess�rio que seja configurado no arquivo appsettings.json para que a aplica��o consiga realizar as mesmas configura��es.

<br>

## M�todos
As reequisi��es para a API seguem os seguintes padr�es:
| M�todo | Descri��o |
|---|---|
| `GET` | Retorna informa��es de um ou mais registros. |
| `POST` | Utilizado para criar um novo registro. |
| `PUT` | Atualiza dados de um registro ou altera sua situa��o. |
| `DELETE` | Remove um registro do sistema. |

<br>

## Projeto Publicado / Reposit�rios

> Vc pode acessar a API BackEnd Publicada clicando aqui: :point_right: https://www.agenda-back.danitavares.dev
>
> <br>
>
> Projeto FrontEnd Publicado: :point_right: https://www.agenda-front.danitavares.dev
>
> Reposit�rio do Projeto FrontEnd: https://github.com/danitavareslobo/AgendaLar-FrontEnd

<br>

## Demonstra��o do Projeto
Esta API ger�ncia os dados presentes no banco de dados agendalarweb.
No banco de dados est�o presentes tr�s tabelas que s�o: Users, People e Phones. Modeladas como na imagem a seguir:
<br>

![agendalarweb](https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/435774d3-9c1e-4da0-afbc-f0aa56b332df)


<br>

https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/01d2a3d3-b181-44e1-af7e-5cbe26108382


<br>

## Acesso ao Projeto
Para utilizar a aplica��o, siga as instru��es abaixo:


#### Pr�-requisitos

� indicado ter instalado em seu computador [VS 2019](https://visualstudio.microsoft.com/pt-br/) ou superior.

Antes de prosseguir, verifique se o seguinte software est� instalado em seu computador: - .NET 8 SDK.
<br>


#### Instru��es

- Clone o reposit�rio para o seu computador;
- Restaure as depend�ncias do projeto: 
```sh 
dotnet restore
```
- Compile o projeto:
```sh 
dotnet build
```
� necess�rio configurar a conex�o com o banco de dados.
  Abra o arquivo appsettings.json localizado no diret�rio raiz do projeto e atualize a string de conex�o `ServerConnection` do banco de dados com as informa��es do seu SQL Server.
- Execute as migra��es do banco de dados para criar as tabelas necess�rias, mais informa��es [aqui](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli);
- A aplica��o est� pronta para execu��o
```sh 
dotnet run
```
<br>

Para acessar e fazer os testes no Swagger, utilize o email: `user1@email.com` e a senha: `User@123` ou cadastre um novo usu�rio. Ap�s isso copie o `accessToken` sem as aspas, clique em `Authorize` e no campo `Value` Digite Bearer d� um espa�o e cole o `accessToken`. Assim ter� acesso para executar todas as op��es dispon�veis.

<br>

## Autoria do Projeto

<div>

> 
> <a href="https://github.com/danitavareslobo"><img src="https://github.com/danitavareslobo/AgendaLar-BackEnd/assets/107322230/0ef94e0e-6396-48bb-b331-5ab0916592d8" width= 120 target="_blank"></a>
<a href="https://github.com/danitavareslobo"> <p >  Daniele Tavares Lobo </p></a>

</div>
