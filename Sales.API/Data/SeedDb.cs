using Microsoft.EntityFrameworkCore;
using Sales.API.Services;
using Sales.Shared.Entities;
using Sales.Shared.Responses;


namespace Sales.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;

        public SeedDb(DataContext context, IApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCetegoriesAsync();
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                Response responseCountries = await _apiService.GetListAsync<CountryResponse>("/v1", "/countries");
                if (responseCountries.IsSuccess)
                {
                    List<CountryResponse> countries = (List<CountryResponse>)responseCountries.Result!;
                    foreach (CountryResponse countryResponse in countries)
                    {
                        Country country = await _context.Countries!.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
                        if (country == null)
                        {
                            country = new() { Name = countryResponse.Name!, States = new List<State>() };
                            Response responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states");
                            if (responseStates.IsSuccess)
                            {
                                List<StateResponse> states = (List<StateResponse>)responseStates.Result!;
                                foreach (StateResponse stateResponse in states!)
                                {
                                    State state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                                    if (state == null)
                                    {
                                        state = new() { Name = stateResponse.Name!, Cities = new List<City>() };
                                        Response responseCities = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
                                        if (responseCities.IsSuccess)
                                        {
                                            List<CityResponse> cities = (List<CityResponse>)responseCities.Result!;
                                            foreach (CityResponse cityResponse in cities)
                                            {
                                                if (cityResponse.Name == "Mosfellsbær" || cityResponse.Name == "Șăulița")
                                                {
                                                    continue;
                                                }
                                                City city = state.Cities!.FirstOrDefault(c => c.Name == cityResponse.Name!)!;
                                                if (city == null)
                                                {
                                                    state.Cities.Add(new City() { Name = cityResponse.Name! });
                                                }
                                            }
                                        }
                                        if (state.CitiesNumber > 0)
                                        {
                                            country.States.Add(state);
                                        }
                                    }
                                }
                            }
                            if (country.StatesNumber > 0)
                            {
                                _context.Countries.Add(country);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }

        private async Task CheckCetegoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Electrónica" });
                _context.Categories.Add(new Category { Name = "Informática" });
                _context.Categories.Add(new Category { Name = "Hogar y Jardín" });
                _context.Categories.Add(new Category { Name = "Moda y Accesorios" });
                _context.Categories.Add(new Category { Name = "Salud y Belleza" });
                _context.Categories.Add(new Category { Name = "Juguetes y Juegos" });
                _context.Categories.Add(new Category { Name = "Deportes y Ocio" });
                _context.Categories.Add(new Category { Name = "Instrumentos musicales" });
                _context.Categories.Add(new Category { Name = "Automoción" });
                _context.Categories.Add(new Category { Name = "Mascotas" });
                _context.Categories.Add(new Category { Name = "Alimentación y Bebidas" });
                _context.Categories.Add(new Category { Name = "Libros y Papelería" });
                _context.Categories.Add(new Category { Name = "Arte y Manualidades" });
                _context.Categories.Add(new Category { Name = "Joyería y Relojes" });
                _context.Categories.Add(new Category { Name = "Viajes y Turismo" });
                _context.Categories.Add(new Category { Name = "Muebles y Decoración" });
                _context.Categories.Add(new Category { Name = "Electrodomésticos" });
                _context.Categories.Add(new Category { Name = "Herramientas y Bricolaje" });
                _context.Categories.Add(new Category { Name = "Cuidado del hogar" });
                _context.Categories.Add(new Category { Name = "Tecnología portátil" });
                _context.Categories.Add(new Category { Name = "Instrumentos de cocina" });
                _context.Categories.Add(new Category { Name = "Maletas y Equipaje" });
                _context.Categories.Add(new Category { Name = "Camping y senderismo" });
                _context.Categories.Add(new Category { Name = "Cámaras y Fotografía" });
                _context.Categories.Add(new Category { Name = "Artículos de oficina" });
                _context.Categories.Add(new Category { Name = "Suministros de mascotas" });
                _context.Categories.Add(new Category { Name = "Juguetes educativos y de aprendizaje" });
                _context.Categories.Add(new Category { Name = "Maquillaje y cosméticos" });
                _context.Categories.Add(new Category { Name = "Productos ecológicos y sostenibles" });
                _context.Categories.Add(new Category { Name = "Productos para el cuidado del bebé" });
                _context.Categories.Add(new Category { Name = "Artículos deportivos acuáticos" });
                _context.Categories.Add(new Category { Name = "Regalos y ocasiones especiales" });
                _context.Categories.Add(new Category { Name = "Videojuegos y consolas" });

                await _context.SaveChangesAsync();
            }
        }
    }
}
