using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Azure.Cosmos;
using my_library_cosmos_db.Exceptions;
using my_library_cosmos_db.Models;

namespace my_library_cosmos_db.Services
{
    public class LibrariesService
    {

        //criando uma variavel privada, para somente ser usada nesta classe. Acrescentei um modificador de campo, o "reandoly",
        //ele é um modificador onde não permite que o valor da variavel seja mudado após ser atribuido, e geralmente o valor é atribuido no construtor, onde atribuirei logo abaixo.
        //esta variavel é do tipo "Container" uma classe da biblioteca Microsoft.Azure.Cosmos;
        //usarei ela para facilitar a instaciação desta classe, onde consiguirei me contectar ao container "libraries" no banco "BooksDB" do cosmosDb
        private readonly Container _container;

        //criando o construtor desta service.
        //acrescentado ao construtor uma variavel do tipo CosmosClient, onde consiguirei acessar os metodos da biblioteca Microsoft.Azure.Cosmos;
        public LibrariesService(CosmosClient client)
        {
            //xumbei a variavel rendonly _container, a obter a conexão com o banco e ao container que irei acessar. assim não precisarei em cada metodo da classe estar instanciando
            _container = client.GetContainer("BooksDB", "libraries");
        }


        //criando o metodo de criar o livro no banco, metodo do tipo async await, ou seja, ele é assincrono, ele não espera uma linha ser feita, para depois ir para a outra
        //enquando poder, ele segue assincronamente, quando tiver o operador await, será em lugares especificos onde o metodo não pode proceguir sem haver uma resposta anterior,
        //criando metodos assincronos a operação fica muito mais rapida.
        public async Task <CreateBookResponseModel> CreateBook (CreateBookRequestModel model)
        {

            //esse metodo aconterá em uma estrutura de DTO, onde cada model fará o seu papel particular, assim uma servindo de request e outra de response, e assim farei a tranferencia de objeto vindo da request, para a model de response, para ser apresentada ao front.
            //usando DTO eu posso apresentar no response apenas as propriedades significativas, além do mais, para uma escalabilidade será mais usual e facil de manutenção.



            if (model == null)
                throw new BadRequestException("object request cannot be null");

            var book = new CreateBookResponseModel
            {
                Id = Guid.NewGuid(),
                LibraryId = model.LibraryId,
                Author = model.Author,
                Title = model.Title,
                Gender = model.Gender,
                NumberPages= model.NumberPages,
                Year = model.Year,
                CreateDate = DateTime.UtcNow,
            };

            var partitionKey = new PartitionKey(model.LibraryId);

            try
            {
                await _container.CreateItemAsync(book, partitionKey);

                return book;
            }
            catch (Exception)
            {

                throw;
            }


           
        }



    }
}
