using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using NUnit.Framework;
using System.IO;
using System.Security.Cryptography;
using OpenQA.Selenium.Firefox;
using System.Collections.ObjectModel;

namespace SeleniumNunit2
{
    [TestFixture]
    class SuiteDePruebas
    {
        IWebDriver chromeDriver;
   
        [OneTimeSetUp]
        public void InicializarSuite()
        {
            Reportar("********** Inicio de ejecución de las pruebas " + DateTime.Now + " **********");
            
            
        }

        [OneTimeTearDown]
        public void FinalizarSuite()
        {
            
            Reportar("********** Fin de la ejecución **********");
        }

        [SetUp]
        public void InicializarCaso()
        {
            chromeDriver = new ChromeDriver(@"C:\drivers");
            chromeDriver.Manage().Window.Maximize();
            chromeDriver.Url = @"https:\\telecentro.com.ar";
            chromeDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            chromeDriver.Navigate();

            WebDriverWait espera = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10));
            
            string btnPackFull = "Pack Full";
            string btnDsp = "//button[contains(@class,'see-availability v-btn v-btn--depressed v-btn--rounded theme--light v-size--x-large white see-availability-plan-id-121')]";
            IWebElement packfull = espera.Until(elemento => chromeDriver.FindElement(By.LinkText(btnPackFull)));
            packfull.Click();
            IWebElement disponibilidad = espera.Until(elemento => chromeDriver.FindElement(By.XPath(btnDsp)));
            disponibilidad.Click();



            //ReadOnlyCollection<IWebElement> disponibilidad = chromeDriver.FindElements(By.ClassName("actions__wrapper"));
            //IWebElement dsp = disponibilidad[0];
            //dsp.Click();

            Reportar("Ingreso a Telecentro, hago click en pack full, tomo el valor de PrecioDesde y entro a VER DISPONIBILDAD");
        }

        [TearDown]
        public void FinalizarCaso()
        {
            chromeDriver.Quit();
        }

        [Test]
        public void Caso1()
        {            

            try
            {           
                Reportar("//////// Comienzo la ejecución del Caso 1 ////////");
                WebDriverWait espera = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10));

                string btnPrecio = "#app > div > main > div > div > div > div.contenedor-ancho-sitio > div > div.plans-list__container > div > div.plans-list-content > div.plans-list > span > div:nth-child(1) > div.plan__main-wrapper > div.plan__actions-wrapper > div.prices__wrapper > div.prices__plan > span.price-label";
                ReadOnlyCollection<IWebElement> precioColeccion = espera.Until(elemento => chromeDriver.FindElements(By.CssSelector(btnPrecio)));
                IWebElement precioPack = precioColeccion[0];
                string precio = precioPack.Text;
                Reportar("Tomo el precio del pack");

                IWebElement list = espera.Until(elemento => chromeDriver.FindElement(By.XPath("//div[@id='app']/div[3]/div/div/div[2]/div/form/div/div/div/div/div/div/div")));
                list.Click();
                IWebElement ciudad = espera.Until(elemento => chromeDriver.FindElement(By.XPath("//div[@id='list-item-318-1']/div/div")));
                ciudad.Click();
                Thread.Sleep(3000);
                Reportar("Selecciono la Ciudad: Avellenada");

                string dom = "//label[contains(text(), 'Domicilio')]/following-sibling::input";
                IWebElement domicilio = espera.Until(elemento => chromeDriver.FindElement(By.XPath(dom)));
                domicilio.Click();
                Thread.Sleep(3000);
                domicilio.SendKeys("mitre");
                Thread.Sleep(3000);
                domicilio.SendKeys(Keys.ArrowDown);
                domicilio.SendKeys(Keys.Enter);
                Reportar("Escribo 'Mitre' como domicilio y elijo la primer opción");

                string btnAceptar = "//button[contains(@class,'v-btn v-btn--depressed v-btn--rounded theme--dark v-size--x-large blue accent-3 trigger-action-see-availability-plan-id-121')]";
                IWebElement aceptar = espera.Until(elemento => chromeDriver.FindElement(By.XPath(btnAceptar)));
                aceptar.Click();
                Thread.Sleep(3000);
                Reportar("Hago click en el botón Aceptar");

                string mensajeRojo = "//div[@class = \"snackbar-message\"]";
                IWebElement msj = espera.Until(elemento => chromeDriver.FindElement(By.XPath(mensajeRojo)));
                Thread.Sleep(2000);
                Reportar("Pregunto si está habilitado en la página actual el elemento web que contiene el mensaje de que no cuentan con el servicio en la zona elegida");
                if (msj.Enabled)
                {
                    Reportar("El valor del plan seleccionado es: " + precio + " la ciudad seleccionada es: 'Avellaneda' " + " y la calle seleccionada es: 'Mitre' " + domicilio.Text + "; el mensaje es: " + msj.Text);
                }
                else
                {
                    Reportar("Error");
                }

                
            }
            catch (AssertionException ae)
            {
                Reportar("Lanzó AssertException: " + ae.Message);

            }
            catch (Exception e)
            {
                Reportar("Se produjo un error:" + e.Message);
                Assert.Fail(e.Message);
            }


        }
        [Test]
        public void Caso2()
        {

            try
            {
                Reportar("//////// Comienzo la ejecución del Caso 2 ////////");
                WebDriverWait espera = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10));

                string btnPrecio = "//span[@class = \"price-label\"]";
                IWebElement precioPrimero = espera.Until(elemento => chromeDriver.FindElement(By.XPath(btnPrecio)));
                string precio = precioPrimero.Text;
                Reportar("Tomo el precio del pack");

                IWebElement list = espera.Until(elemento => chromeDriver.FindElement(By.XPath("//div[@id='app']/div[3]/div/div/div[2]/div/form/div/div/div/div/div/div/div")));
                list.Click();
                IWebElement ciudad = espera.Until(elemento => chromeDriver.FindElement(By.XPath("//div[18]/div/div"))); 
                ciudad.Click();
                Thread.Sleep(3000);
                Reportar("Selecciono la Ciudad: La Matanza");

                string dom = "//label[contains(text(), 'Domicilio')]/following-sibling::input";
                IWebElement domicilio = espera.Until(elemento => chromeDriver.FindElement(By.XPath(dom)));
                domicilio.Click();
                Thread.Sleep(3000);
                domicilio.SendKeys("mitre");
                Thread.Sleep(3000);
                domicilio.SendKeys(Keys.ArrowDown);
                domicilio.SendKeys(Keys.Enter);
                Reportar("Escribo 'Mitre' como domicilio y elijo la primer opción");

                string btnAceptar = "//button[contains(@class,'v-btn v-btn--depressed v-btn--rounded theme--dark v-size--x-large blue accent-3 trigger-action-see-availability-plan-id-121')]";
                IWebElement aceptar = espera.Until(elemento => chromeDriver.FindElement(By.XPath(btnAceptar)));
                aceptar.Click();
                Thread.Sleep(3000);
                Reportar("Hago click en el botón Aceptar");


                string selectorPreciofinal = "//div[@class = \"list-item monthly-billing\"]/div[@class = \"result-container\"]";
                IWebElement precio2 = espera.Until(elemento => chromeDriver.FindElement(By.XPath(selectorPreciofinal)));
                string precioFinal = precio2.Text;
                precioFinal = precioFinal.Replace(" ", "");
                //Assert.IsTrue(precio == precioFinal);
                if (precio == precioFinal)
                {
                    Reportar("El precio del Plan seleccionado es: " + precio + " y el valor del plan seleccionado en la segunda pantalla es: " + precioFinal);
                }

            }
            catch (AssertionException ae)
            {
                Reportar("Lanzó AssertException: " + ae.Message);


            }
            catch (Exception e)
            {
                Reportar("Se produjo un error:" + e.Message);
                Assert.Fail(e.Message);
            }

        }
        
       
     


        private void Reportar(string texto)
        {
            TextWriter escritor = null;
            try
            {
                escritor = new StreamWriter(@"C:\Reportes\Reporte.txt", true);
                escritor.WriteLine(texto);
            }
            finally
            {
                escritor.Close();
            }
        }
        
        
    }
}
