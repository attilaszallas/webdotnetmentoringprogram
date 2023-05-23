using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ConsoleHttpClient
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static void ShowEntities(IEnumerable<string> entities)
        {
            foreach (var entity in entities) 
            {
                Console.WriteLine(entity);
            }
        }

        static async Task<IEnumerable<string>> GetAsync(string path)
        {
            IEnumerable<string> entities = null;
            HttpResponseMessage response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                entities = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            }

            return entities;
        }

        static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            string categoriesApiUri = "https://localhost:7140/api/CategoriesApi/GetCategories";
            string productsApiUri = "https://localhost:7140/api/ProductsApi/GetProducts";

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var categories = await GetAsync(categoriesApiUri);

                Console.WriteLine("\n Categories: \n");
                ShowEntities(categories);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                var products = await GetAsync(productsApiUri);

                Console.WriteLine("\n Products: \n");
                ShowEntities(products);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
