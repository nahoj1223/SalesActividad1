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
                _context.Categories.Add(new Category { Name = " Accesorios electrónicos" });
                _context.Categories.Add(new Category { Name = " Accesorios para computadoras y laptops" });
                _context.Categories.Add(new Category { Name = " Accesorios para teléfonos móviles y tabletas" });
                _context.Categories.Add(new Category { Name = " Alimentos y bebidas" });
                _context.Categories.Add(new Category { Name = " Alimentos y golosinas para mascotas" });
                _context.Categories.Add(new Category { Name = " Artículos de arte y manualidades" });
                _context.Categories.Add(new Category { Name = " Artículos de caza y tiro deportivo" });
                _context.Categories.Add(new Category { Name = " Artículos de colección" });
                _context.Categories.Add(new Category { Name = " Artículos de cuidado personal y belleza" });
                _context.Categories.Add(new Category { Name = " Artículos de decoración del hogar" });
                _context.Categories.Add(new Category { Name = " Artículos de joyería y relojes" });
                _context.Categories.Add(new Category { Name = " Artículos de limpieza y mantenimiento del hogar" });
                _context.Categories.Add(new Category { Name = " Artículos de papelería y oficina" });
                _context.Categories.Add(new Category { Name = " Artículos de regalo y tarjetas de felicitación" });
                _context.Categories.Add(new Category { Name = " Artículos de tecnología y electrónica" });
                _context.Categories.Add(new Category { Name = " Artículos de temporada" });
                _context.Categories.Add(new Category { Name = " Artículos deportivos y de fitness" });
                _context.Categories.Add(new Category { Name = " Artículos para el baño" });
                _context.Categories.Add(new Category { Name = " Artículos para manualidades y hobbies" });
                _context.Categories.Add(new Category { Name = " Bebidas alcohólicas y no alcohólicas" });
                _context.Categories.Add(new Category { Name = " Bolsos y carteras" });
                _context.Categories.Add(new Category { Name = " Calzado para hombre" });
                _context.Categories.Add(new Category { Name = " Calzado para hombres" });
                _context.Categories.Add(new Category { Name = " Calzado para mujer" });
                _context.Categories.Add(new Category { Name = " Calzado para niños" });
                _context.Categories.Add(new Category { Name = " Cámaras y equipo fotográfico" });
                _context.Categories.Add(new Category { Name = " Cómics" });
                _context.Categories.Add(new Category { Name = " Computadoras y accesorios" });
                _context.Categories.Add(new Category { Name = " Cuidado de la piel" });
                _context.Categories.Add(new Category { Name = " Cuidado del cabello" });
                _context.Categories.Add(new Category { Name = " Decoración del hogar" });
                _context.Categories.Add(new Category { Name = " Discos de vinilo y CD's" });
                _context.Categories.Add(new Category { Name = " Dispositivos de sonido y parlantes" });
                _context.Categories.Add(new Category { Name = " Electrodomésticos" });
                _context.Categories.Add(new Category { Name = " Electrónica" });
                _context.Categories.Add(new Category { Name = " Equipo de pesca" });
                _context.Categories.Add(new Category { Name = " Equipo para acampar y hacer senderismo" });
                _context.Categories.Add(new Category { Name = " Figuras de acción" });
                _context.Categories.Add(new Category { Name = " Herramientas manuales y eléctricas" });
                _context.Categories.Add(new Category { Name = " Herramientas y suministros para jardinería" });
                _context.Categories.Add(new Category { Name = " Iluminación" });
                _context.Categories.Add(new Category { Name = " Impresoras y suministros de oficina" });
                _context.Categories.Add(new Category { Name = " Instrumentos musicales" });
                _context.Categories.Add(new Category { Name = " Joyería" });
                _context.Categories.Add(new Category { Name = " Juegos de mesa" });
                _context.Categories.Add(new Category { Name = " Juguetes y juegos" });
                _context.Categories.Add(new Category { Name = " Lentes de sol" });
                _context.Categories.Add(new Category { Name = " Libros" });
                _context.Categories.Add(new Category { Name = " Libros y revistas" });
                _context.Categories.Add(new Category { Name = " Maquillaje" });
                _context.Categories.Add(new Category { Name = " Muebles" });
                _context.Categories.Add(new Category { Name = " Muebles y decoración de jardín" });
                _context.Categories.Add(new Category { Name = " Objetos de arte" });
                _context.Categories.Add(new Category { Name = " Partituras y libros de música" });
                _context.Categories.Add(new Category { Name = " Películas y series de televisión en DVD o Blu-ray" });
                _context.Categories.Add(new Category { Name = " Peluches" });
                _context.Categories.Add(new Category { Name = " Perfumes y fragancias" });
                _context.Categories.Add(new Category { Name = " Piezas de automóviles" });
                _context.Categories.Add(new Category { Name = " Piezas de bicicletas" });
                _context.Categories.Add(new Category { Name = " Piezas de motocicletas" });
                _context.Categories.Add(new Category { Name = " Plantas y flores" });
                _context.Categories.Add(new Category { Name = " Productos de cuidado personal" });
                _context.Categories.Add(new Category { Name = " Productos de salud y bienestar" });
                _context.Categories.Add(new Category { Name = " Productos electrónicos para el hogar" });
                _context.Categories.Add(new Category { Name = " Productos para mascotas" });
                _context.Categories.Add(new Category { Name = " Relojes" });
                _context.Categories.Add(new Category { Name = " Ropa para hombre" });
                _context.Categories.Add(new Category { Name = " Ropa para mujer" });
                _context.Categories.Add(new Category { Name = " Ropa para niños" });
                _context.Categories.Add(new Category { Name = " Ropa y accesorios para hombres" });
                _context.Categories.Add(new Category { Name = " Ropa y accesorios para mujeres" });
                _context.Categories.Add(new Category { Name = " Ropa y accesorios para niños" });
                _context.Categories.Add(new Category { Name = " Snacks y alimentos para picar" });
                _context.Categories.Add(new Category { Name = " Software y videojuegos" });
                _context.Categories.Add(new Category { Name = " Sombreros y gorras" });
                _context.Categories.Add(new Category { Name = " Suministros de pintura y dibujo" });
                _context.Categories.Add(new Category { Name = " Suministros para acuariofilia" });
                _context.Categories.Add(new Category { Name = " Suministros para animales de granja" });
                _context.Categories.Add(new Category { Name = " Suministros para artes marciales" });
                _context.Categories.Add(new Category { Name = " Suministros para automóviles" });
                _context.Categories.Add(new Category { Name = " Suministros para aves" });
                _context.Categories.Add(new Category { Name = " Suministros para aviación" });
                _context.Categories.Add(new Category { Name = " Suministros para barcos y embarcaciones" });
                _context.Categories.Add(new Category { Name = " Suministros para bicicletas" });
                _context.Categories.Add(new Category { Name = " Suministros para bienestar y nutrición" });
                _context.Categories.Add(new Category { Name = " Suministros para boxeo y lucha" });
                _context.Categories.Add(new Category { Name = " Suministros para caballos" });
                _context.Categories.Add(new Category { Name = " Suministros para camping" });
                _context.Categories.Add(new Category { Name = " Suministros para caza" });
                _context.Categories.Add(new Category { Name = " Suministros para cuidado de la salud" });
                _context.Categories.Add(new Category { Name = " Suministros para deportes acuáticos" });
                _context.Categories.Add(new Category { Name = " Suministros para deportes al aire libre" });
                _context.Categories.Add(new Category { Name = " Suministros para deportes de invierno" });
                _context.Categories.Add(new Category { Name = " Suministros para educación y tutorías" });
                _context.Categories.Add(new Category { Name = " Suministros para ejercicios en casa" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de atención al paciente" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de atención de emergencia" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de atención médica en el hogar" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de automatización industrial" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de belleza y estética" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de catering y eventos" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de cocina y restaurante" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de construcción" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de construcción de carreteras" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de cuidado a largo plazo" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de diagnóstico" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de diagnóstico por imágenes" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de empaque y envío" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de energía eólica" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de energía geotérmica" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de energía hidráulica" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de energía nuclear" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de energía renovable" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de energía solar" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de energía térmica" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de iluminación LED" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de investigación y desarrollo" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de jardinería y paisajismo" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de laboratorio" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de limpieza industrial" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de minería" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de monitoreo y vigilancia" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de panadería y pastelería" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de procesamiento de alimentos" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de protección personal" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de refrigeración y aire acondicionado" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de rehabilitación" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de robótica" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de saneamiento" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de seguridad" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de seguridad alimentaria" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de seguridad contra incendios" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de seguridad contra robos" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de servicio de ambulancia" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de tatuaje y perforación corporal" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de tecnología de la información" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de telecomunicaciones" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos de terapia" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos dentales" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos médicos" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos pesados" });
                _context.Categories.Add(new Category { Name = " Suministros para equipos veterinarios" });
                _context.Categories.Add(new Category { Name = " Suministros para fisioterapia" });
                _context.Categories.Add(new Category { Name = " Suministros para formación profesional" });
                _context.Categories.Add(new Category { Name = " Suministros para gimnasios y entrenamiento" });
                _context.Categories.Add(new Category { Name = " Suministros para insectos" });
                _context.Categories.Add(new Category { Name = " Suministros para la higiene y el cuidado de mascotas" });
                _context.Categories.Add(new Category { Name = " Suministros para masajes y relajación" });
                _context.Categories.Add(new Category { Name = " Suministros para mascotas" });
                _context.Categories.Add(new Category { Name = " Suministros para meditación y mindfulness" });
                _context.Categories.Add(new Category { Name = " Suministros para motocicletas" });
                _context.Categories.Add(new Category { Name = " Suministros para peces" });
                _context.Categories.Add(new Category { Name = " Suministros para pesca" });
                _context.Categories.Add(new Category { Name = " Suministros para psicoterapia" });
                _context.Categories.Add(new Category { Name = " Suministros para reparaciones del hogar" });
                _context.Categories.Add(new Category { Name = " Suministros para reptiles" });
                _context.Categories.Add(new Category { Name = " Suministros para roedores" });
                _context.Categories.Add(new Category { Name = " Suministros para senderismo" });
                _context.Categories.Add(new Category { Name = " Suministros para terapia de pareja y familia" });
                _context.Categories.Add(new Category { Name = " Suministros para terapia del habla" });
                _context.Categories.Add(new Category { Name = " Suministros para terapia física" });
                _context.Categories.Add(new Category { Name = " Suministros para terapia ocupacional" });
                _context.Categories.Add(new Category { Name = " Suministros para terapias alternativas" });
                _context.Categories.Add(new Category { Name = " Suministros para terrarios" });
                _context.Categories.Add(new Category { Name = " Suministros para tratamiento del dolor" });
                _context.Categories.Add(new Category { Name = " Suministros para yoga y pilates" });
                _context.Categories.Add(new Category { Name = " Teléfonos celulares y accesorios" });

                await _context.SaveChangesAsync();
            }
        }
    }
}
