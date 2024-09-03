# CarrefourFluxoCaixa
Projeto desenvolvido com .NET 7.0
Instruções de como utilizar web api estão disponíveis em Swagger. Se executar a partir de Visual Studio faça em modo administrador para carregar comentários em Swagger.

**Desing Patterns
SOLID Principles: baseado em interfaces para serviços e entidades

Microserviço: independente para time, permite escalar horizontalmente e verticalmente

##CQRS (Não inteiramente já que não existe uma outra base de dados para read na solução, então não tem implementação de Query Results. Existe commands e validation commands apenas para Account (já que é um teste), business layer está em application service com validação lá também, repository pattern, view model e models): Consultas e persistência em database realizada em repositórios (Persistência), contratos repositórios, commands (Account), validations commands(Account) e entities em Domínio, Serviços invocam commands e repositories através de contratos, controller invocam serviços

##Saga: FluxoCaixaConsolidado service RabbitMQ

##Repository: EF

##Strategy: EF depende de abstrações

##Retry: tentativa de reconexão à base de dados

##AutoMapper: mapeamento entre viewModel, entidades, modelRequest, commands

##Annotations: entidades e view models com características de campo e validação

##EntityFramework: EF Core 7, CodeFirst

##AMQP Rabbit MQ: Mensageria pode ser ativada ou desativada em arquivo de appsettings, mas se ativada deve ser instalada versão (3.12.2 ou 3.13.7), em projeto RabbitMQ.Client 6.5.0

Download Erlang OTP 27.0.1

##Unit Tests: Utilizado Moq e VisualStudioUnitTests. Realizado unit testes com padrão AAA (arrange act assert) de um serviço ApplicationServiceBalanceUnitTest. Cobre métodos, exceções 

##Integration Tests: AutoMapper, IConfiguration, Repositories, Serviços

##Monitoramento Application insights

##SQL Server 2016 database gerado a partir de migrations (executar seguintes comandos na pasta da solução)
dotnet tool install --global dotnet-ef

dotnet ef database update FluxoCaixa --project Persistence -s CarrefourFluxoCaixa -c FluxoCaixaContext --verbose

dotnet ef database update 20230730182027_RecordTypeToChar --project Persistence -s CarrefourFluxoCaixa -c FluxoCaixaContext --verbose

dotnet ef database update 20230730182658_Comments --project Persistence -s CarrefourFluxoCaixa -c FluxoCaixaContext --verbose

##IIS padrão de uso

##Docker: deve ser realizado publish, gerar imagem e criar container, incluir atributo para permitir consulta a serviços externos --network host, connection string deve ser atualizada em appsettings.json para container linux (exemplo:
"ConnectionStrings": {
    "DefaultConnection": "Data Source=host.docker.internal,1433;Initial Catalog=FluxoCaixa;Integrated Security=False;User ID=user;Password=password;Encrypt=False;"
  })
alterar AMQP hostname para host.docker.internal

Para publicar versão da aplicação faça na pasta da solução:
Dotnet publish -c Debug

Gerar imagem em docker (na pasta da solução faça):
docker build -f ".\CarrefourFluxoCaixa\Dockerfile" --force-rm -t carrefour ".\"

Criar container a partir de imagem:
docker run -dt -e "ASPNETCORE_ENVIRONMENT=Development" -e "ASPNETCORE_LOGGING__CONSOLE__DISABLECOLORS=true"  -p49155:80 --name carrefour_development carrefour:latest --network host

Fluxo: ![image](https://github.com/davilinfo/Minsait-CarrefourFluxoCaixa/assets/18128361/01dd7353-4580-49df-a817-8b113c30efee)





