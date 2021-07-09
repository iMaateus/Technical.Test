
# .NET Web API

Web API Teste Técnico.

Endpoint de demonstração e documentação: 
## https://api-technical-test-seventh.herokuapp.com/swagger/index.html


## Estrutura
• Linguagem: C# .NET5

• Banco de dados: MongoDB (Atlas)

• Repositório de arquivos binários: AWS S3

• Reciclagem de videos: AWS Lambda e AWS SQS

## Desenvolvimento

CRUD simples de Servidor e Video feito via API para o MongoDB.

Upload de arquivo via feito integração .NET com AWS para S3.

Remover vídeo dispara evento para AWS SQS que ao chegar em seu processamento executa uma função (AWS Lambda) para remover o arquivo do repositório (AWS S3).

Para reciclagem o mesmo ocorre, com a diferença que a exclusão no banco de dados é feita via AWS Lambda. 

## Executar

```bash
dotnet run
```

## Considerações

Ajustar conexões com AWS e MongoBD em appsettings.Production e appsettings.Development

No projeto com Lambda Function ajustar os mesmos valores em suas constantes.
