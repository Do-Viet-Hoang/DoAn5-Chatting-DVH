using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn5_NetCore.Models;
using DoAn5_NetCore.Services;
using Newtonsoft.Json;

namespace DoAn5_NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeessagesController : ControllerBase
    {
        private readonly ChatingContext _context;
        private readonly IFileService _fileService;

        public MeessagesController(ChatingContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet("get-mess-box")]
        public ActionResult<Meessages> GetMessageBox()
        {
            Guid userId = Guid.Parse(User.Identity.Name);
            var box = _context.MessageBox
                .Select(mess => new
                {
                    fromId = mess.FromUserId,
                    toUser = new
                    {
                        mess.ToUser.UsersId,
                        mess.ToUser.Avatar,
                        mess.ToUser.FullName,
                    },
                    lastMessage = _context.Meessages.OrderByDescending(m => m.CreatedDateTime)
                        .Select(mess => new
                        {
                            mess.FromUserId,
                            mess.ToUserId,
                            mess.Content,
                            mess.CreatedDateTime,
                        })
                        .FirstOrDefault(messx => (messx.FromUserId == mess.FromUserId && messx.ToUserId == mess.ToUserId) || (messx.FromUserId == mess.ToUserId && messx.ToUserId == mess.FromUserId))
                })
                .Where(mess => mess.fromId == userId)
                .OrderByDescending(mess => mess.lastMessage.CreatedDateTime);

            return Ok(box);
        }

        [HttpGet("get-all-mess")]
        public ActionResult<Meessages> GetAllMessage(Guid from_id, Guid to_id)
        {
            var box = _context.Meessages
                .OrderByDescending(m => m.MessageId)
                .Select(mess => new
                {
                    fromUser = new
                    {
                        id = mess.FromUserId,
                        mess.FromUser.FullName,
                        mess.FromUser.Avatar
                    },
                    toUser = new
                    {
                        id = mess.ToUserId,
                        mess.ToUser.FullName,
                        mess.ToUser.Avatar
                    },
                    mess.Content,
                    mess.MediaFilePath,
                    mess.CreatedDateTime,
                })
                .Where(mess => (mess.fromUser.id == from_id && mess.toUser.id == to_id) || (mess.fromUser.id == to_id && mess.toUser.id == from_id));
            return Ok(box);
        }

        [HttpPost("send-message")]
        public async Task<ActionResult<Meessages>> SendMessage(MeessagesClone messages)
        {
            Guid userId = Guid.Parse(User.Identity.Name);

            // them tin nhan
            var message = messages.get();
            message.FromUserId = userId;

            string[] medias = new string[0];

            foreach (var item in messages.MediaFilePath)
            {
                var file = _fileService.WriteFileBase64(item);
                Array.Resize(ref medias, medias.Length + 1);
                medias[medias.Length - 1] = file;
            }

            var json = JsonConvert.SerializeObject(medias);

            message.MediaFilePath = json;
            message.CreatedDateTime = DateTime.Now;

            _context.Meessages.Add(message);
            await _context.SaveChangesAsync();

            // them loi tat tin nhan
            // nguoi gui
            if (!_context.MessageBox.Any(msgBox => msgBox.FromUserId == message.FromUserId && msgBox.ToUserId == message.ToUserId))
            {
                _context.MessageBox.Add(new MessageBox()
                {
                    FromUserId = message.FromUserId,
                    ToUserId = message.ToUserId,
                });
                await _context.SaveChangesAsync();
            }

            // nguoi nhan
            if (!_context.MessageBox.Any(msgBox => msgBox.FromUserId == message.ToUserId && msgBox.ToUserId == message.FromUserId))
            {
                _context.MessageBox.Add(new MessageBox()
                {
                    FromUserId = message.ToUserId,
                    ToUserId = message.FromUserId,
                });
                await _context.SaveChangesAsync();
            }

            message.FromUser = _context.Users.Find(message.FromUserId);
            message.ToUser = _context.Users.Find(message.ToUserId);

            return Ok(new
            {
                fromUser = new
                {
                    id = message.FromUserId,
                    message.FromUser.FullName,
                    message.FromUser.Avatar
                },
                toUser = new
                {
                    id = message.ToUserId,
                    message.ToUser.FullName,
                    message.ToUser.Avatar
                },
                message.Content,
                message.MediaFilePath,
            });
        }

        // GET: api/Meessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meessages>>> GetMeessages()
        {
            return await _context.Meessages.ToListAsync();
        }

        // GET: api/Meessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Meessages>> GetMeessages(Guid id)
        {
            var meessages = await _context.Meessages.FindAsync(id);

            if (meessages == null)
            {
                return NotFound();
            }

            return meessages;
        }

        // PUT: api/Meessages/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeessages(Guid id, Meessages meessages)
        {
            if (id != meessages.MessageId)
            {
                return BadRequest();
            }

            _context.Entry(meessages).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeessagesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Meessages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Meessages>> PostMeessages(Meessages meessages)
        {
            _context.Meessages.Add(meessages);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeessages", new { id = meessages.MessageId }, meessages);
        }

        // DELETE: api/Meessages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Meessages>> DeleteMeessages(Guid id)
        {
            var meessages = await _context.Meessages.FindAsync(id);
            if (meessages == null)
            {
                return NotFound();
            }

            _context.Meessages.Remove(meessages);
            await _context.SaveChangesAsync();

            return meessages;
        }

        private bool MeessagesExists(Guid id)
        {
            return _context.Meessages.Any(e => e.MessageId == id);
        }
    }
}
