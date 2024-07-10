
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Route("[Action]")]
[ApiController]
public class MessageController : Controller
{
    private readonly Context db;
    public readonly MyFilter _myFilter;

    public MessageController(Context _db,MyFilter myFilter)
    {
        db = _db;
        _myFilter=myFilter;
    }

    

    [HttpPost]
    public IActionResult AddMessage(DtoMessage message)
    {
        using (var transaction = db.Database.BeginTransaction())
        {
            try
            {
                var newMessage = new Messages
                {
                    SerialNumber = message.SerialNumber,
                    SenderUserId = message.SenderUserId,
                    Subject = message.Subject,
                    BodyText = message.BodyText,
                    CreateDateTime = DateTime.Now
                };

                db.Messages_tbl.Add(newMessage);
                db.SaveChanges();

                int messageId = Convert.ToInt32(newMessage.Id);

                foreach (var item in message.Resivers)
                {
                    db.Recivers_tbl.Add(new Recivers
                    {
                        ReciverId = item.ReciverId,
                        MessageId = messageId,
                        Type = item.Type,
                        CreateDateTime = DateTime.Now
                    });
                }

                foreach (var item in message.Atachments)
                {
                    db.Attecheds_tbl.Add(new Atteched
                    {
                        FileName = item.FileName,
                        MessageId = messageId,
                        FilePath = item.FilePath,
                        FileType = item.FileType,
                        CreateDateTime = DateTime.Now
                    });
                }

                db.SaveChanges();
                transaction.Commit();

                return Ok("Successful");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }




 [HttpGet]

    public async Task<IActionResult> Getallmessage([FromQuery] PaginationFilter filter, [FromQuery] Messages msg)
    {
       try
        {
            //AddLog
            

            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = 400, message = "عملیات با خطا مواجه شد" });
            }

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var pagedData = await _myFilter.PerformOperation(msg)
               .Include(x=> x.SenderUser)
               .Include(x=> x.Atteched)
               .Include(x=> x.Recivers)
                       .ThenInclude(r => r.Reciver)

                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            var totalRecords = pagedData.Count();
            var totalCount = await _myFilter.PerformOperation(msg).CountAsync();
            var totalPage = (int)Math.Ceiling(totalCount / (double)validFilter.PageSize);
            return Ok(new PagedResponse<List<Messages>>(pagedData, validFilter.PageNumber, validFilter.PageSize, totalPage, totalCount));
        }
        catch (Exception ex)
        {
            return BadRequest(new { status = 400, message = "عملیات با خطا مواجه شد" + ex.Message });
        }
    }

    [HttpDelete]
    public IActionResult DeleteMessage(int MessageId){
        db.Messages_tbl.Remove(db.Messages_tbl.Find(MessageId));
        db.SaveChanges();
        foreach (var item in db.Recivers_tbl.Where(x=> x.MessageId == MessageId).ToList())
        {
            db.Recivers_tbl.Remove(item);
            db.SaveChanges();
        }
        foreach (var item in db.Attecheds_tbl.Where(x=> x.MessageId == MessageId).ToList())
        {
            db.Attecheds_tbl.Remove(item);
            db.SaveChanges();
        }
        return Ok();
    }



}