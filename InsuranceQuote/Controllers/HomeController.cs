using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuranceQuote.Models;
using System.Data.SqlClient;
using System.Data;


namespace InsuranceQuote.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(string firstName, string lastName, string emailAddress, DateTime dateOfBirth, int carYear, string carMake, string carModel,
                                    bool hasDUI, int numberOfSpeedingTickets, string fullCoverageOrLiability)
        {
            
            
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) || 
                dateOfBirth == null || string.IsNullOrEmpty(carYear.ToString()) || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel) || 
                string.IsNullOrEmpty(hasDUI.ToString()) || string.IsNullOrEmpty(numberOfSpeedingTickets.ToString()) || string.IsNullOrEmpty(fullCoverageOrLiability))
            {
                return View("~/Views/Shared/Error.cshtml");
            }
            else
            {
                using (InsuraceQuoteEntities db = new InsuraceQuoteEntities())
                {
                    
                    var signup = new SignUp();
                    signup.FirstName = firstName;
                    signup.LastName = lastName;
                    signup.EmailAddress = emailAddress;
                    signup.DateOfBirth = dateOfBirth;
                    signup.CarYear = carYear;
                    signup.CarMake = carMake;
                    signup.CarModel = carModel;
                    signup.HasDUI = hasDUI;
                    signup.NumberOfSpeedingTickets = numberOfSpeedingTickets;
                    signup.FullCoverageOrLiability = fullCoverageOrLiability;
                    
                    decimal ageQuote = new decimal();
                    if(DateTime.Now.Year - dateOfBirth.Year < 25)
                    {
                        ageQuote = 25;
                    }
                    else if (DateTime.Now.Year - dateOfBirth.Year < 18)
                    {
                        ageQuote = 100;
                    }
                    else if (DateTime.Now.Year - dateOfBirth.Year > 100)
                    {
                        ageQuote = 25;
                    }
                    else
                    {
                        ageQuote = 0;
                    }

                    decimal yearQuote = new decimal();
                    if(carYear < 2000)
                    {
                        yearQuote = 25;
                    }
                    else if(carYear > 2015)
                    {
                        yearQuote = 25;
                    }
                    else
                    {
                        yearQuote = 0;
                    }

                    decimal makeQuote = new decimal();
                    if(carMake == "Porsche")
                    {
                        makeQuote = 25;
                    }
                    else if(carMake == "Porsche" && carModel == "911 Carrera")
                    {
                        makeQuote = 50;
                    }
                    decimal speedingTicketQuote = numberOfSpeedingTickets * 10;
                    decimal hasDUIQuote = new decimal();
                    if(hasDUI == true)
                    {
                        hasDUIQuote = 1.25m;
                    }
                    else
                    {
                        hasDUIQuote = 1;
                    }

                    decimal fullCoverageQuote = new decimal();
                    if(fullCoverageOrLiability == "Full Coverage")
                    {
                        fullCoverageQuote = 1.5m;
                    }
                    else
                    {
                        fullCoverageQuote = 1;
                    }

                    decimal quote = ((yearQuote + ageQuote + makeQuote + speedingTicketQuote + 50) * hasDUIQuote) * fullCoverageQuote;

                    signup.InsuranceQuote = quote;
                                                          

                    db.SignUps.Add(signup);
                    db.SaveChanges();
                }


                return View("Success");
            }
           
        }
        
    }
}