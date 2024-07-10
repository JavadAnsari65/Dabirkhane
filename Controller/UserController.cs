using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
[Route("[Action]")]
[ApiController]
public class UserController : Controller
{
    private readonly string salt = "S@lt?";
    private readonly Context db;
    public UserController(Context _db)
    {
        db = _db;
    }

    [HttpPost]
    public IActionResult Register(DtodUser user)
    {
        db.Users_tbl.Add(new Users
        {
            Username = user.Username.ToLower(),
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password + salt + user.Username.ToLower()),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Phone = user.Phone,
            Addres = user.Addres,
            NatinalCode = user.NatinalCode,
            PerconalCode = user.PerconalCode,
            CreateDateTime = DateTime.Now
        });
        db.SaveChanges();
        return Ok("Succesful !");
    }

    [HttpPost]
    public IActionResult Login(string Username, string Password)
    {
        Users check = db.Users_tbl.FirstOrDefault(x => x.Username == Username.ToLower());
        if (check == null)
        {
            return NotFound($"{Username} not found");
        }
        else if (!BCrypt.Net.BCrypt.Verify(Password + salt + Username.ToLower(), check.Password))
        {
            return Ok("Invalid Password !");
        }
        else
        {
            return Ok(CreateToken(Username.ToLower()));
        }
    }

    [HttpPut]
    public IActionResult ResetPassword(string Username, string NatinalCode)
    {
        Users check = db.Users_tbl.FirstOrDefault(x => x.Username == Username.ToLower() && x.NatinalCode == NatinalCode);
        if (check == null)
        {
            return Ok("Invalid Data");
        }

        // sms check
        smsUser request = db.sms_tbl.FirstOrDefault(x => x.UserId == check.Id);

        if (request != null && DateTime.Now.AddMinutes(-10) < request.CreateDateTime)
        {
            return Ok("you Must Wait 10 min");
        }

        if (request != null)
        {
            db.sms_tbl.Remove(request);
        }
        Random random = new Random();
        smsUser newSms = new smsUser
        {
            SmsCode = random.Next(100000, 999999).ToString(),
            UserId = (int)check.Id,
            IsValid = true,
            CreateDateTime = DateTime.Now
        };
        db.sms_tbl.Add(newSms);
        db.SaveChanges();
        return Ok(SmsCode(newSms.SmsCode, check.Phone));
    }

    [HttpPut]
    public IActionResult VerifyPassword(string Username, string NewPass, string Code)
    {
        Users check = db.Users_tbl.FirstOrDefault(x => x.Username == Username.ToLower());
        if (check == null)
        {
            return Ok("Invalid User");
        }

        //sms Check
        smsUser smsCheck = db.sms_tbl.FirstOrDefault(x => x.UserId == check.Id);
        if (smsCheck == null)
        {
            return Ok("Haven't Code Requset. try Reset First");
        }
        else if (DateTime.Now.AddMinutes(-10) < smsCheck.CreateDateTime)
        { //Time Passed
            db.sms_tbl.Remove(smsCheck);
            return Ok("Code Time Expire ... Try again");
        }
        else if (smsCheck.IsValid == true)
        {
            if (Code == smsCheck.SmsCode)
            {
                check.Password = BCrypt.Net.BCrypt.HashPassword(NewPass + salt + Username.ToLower());
                db.Users_tbl.Update(check);
                db.sms_tbl.Remove(smsCheck);
                db.SaveChanges();
                return Ok("Sucssesful");
            }
            else
            {
                smsCheck.IsValid = false;
                db.sms_tbl.Update(smsCheck);
                db.SaveChanges();
                return Ok("Code is Invalid");
            }
        }
        return Ok("YOU CANT SEE THIS");
    }


    private string CreateToken(string Username)
    {
        return "token";
    }
    private string SmsCode(string Code, string Phone)
    {
        // Expend Sms Metode Latar
        return $"{Code} Sent to {Phone} .";
    }
}
