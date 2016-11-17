﻿using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using ColonyServer.App_Data;
using ColonyServer.Models;
using System.Collections.Generic;

namespace ColonyServer.Controllers
{
    public class UsersController : ApiController
    {
        AlertDialog alertDialog = new AlertDialog(); 
        User user = new User();
        
        // GET: api/Users/5
        //Find user 
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string id)
        {
            user=user.FindUser(id);
            
            if ( user != null)
            {
                alertDialog.Code = "user_success";
                alertDialog.Message = user.UserName;
                
            }
            else
            {
                alertDialog.Code = "user_error";
                alertDialog.Message = "User not Found";
            }
            return Ok(alertDialog);

            
            }

        // PuT: api/Users
        // login/new user
        [ResponseType(typeof(User))]
        public IHttpActionResult PutUser(User user)
        {
            if (ModelState.IsValid)
            {
                // check if user Exsist
                if (!user.NewUser())
                {
                    //user Exsist try login.
                    alertDialog.Message = user.FindUser(user.Number).LoginUser(user);
                    if (alertDialog.Message.Equals("User login"))
                    {
                        alertDialog.Code = "log_success";
                    }
                }
                else
                {
                    //if user note Exsist Create new User.
                    alertDialog.Message = "user Created";
                    alertDialog.Code = "reg_success";

                }
            }
            else
            {
                 //check if was error in validation
                alertDialog.Message = string.Join(" \n", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                alertDialog.Code = "val_error";


            }
                    
          
            return Ok(alertDialog);
        }

        // DELETE: api/Users/name
        //Delete user
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(string id)
        {
           
            if(!user.FindUser(id).DeleteUser())
            {
                return NotFound();

            }
            return Ok("user Delteted !");
        }

        [Route("api/User/contact")]
        [HttpPost]
        public IHttpActionResult PostContact([FromBody] List<Contact> contacts)
        {
            int i;
            List<Contact> dbContact = new List<Contact>();

            for (i = 0; i < contacts.Count; i++)
            {
                if (user.FindUser(contacts[i].Number)!=null)
                {
                    dbContact.Add(contacts[i]);
                }
         
            }           

            return Ok(dbContact);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

    }
}