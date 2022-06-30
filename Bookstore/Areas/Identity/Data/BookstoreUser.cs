using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Areas.Identity.Data;

// Add profile data for application users by adding properties to the BookstoreUser class
public class BookstoreUser : IdentityUser
{
    [PersonalData]
    public int user_ID  { get; set; }
}

