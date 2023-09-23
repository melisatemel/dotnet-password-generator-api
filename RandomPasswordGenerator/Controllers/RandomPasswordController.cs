using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RandomPasswordGenerator.Controllers;

public class RandomPasswordRequest
{
    [Required(ErrorMessage = "Bu alanın girilmesi zorunlu!")]
    [Range(1,double.MaxValue, ErrorMessage ="En az 1 sayı girmelisiniz!")]

    public int PasswordLength { get; set; }
    public bool IsIncludeNumbers { get; set; }
    public bool IsIncludeLetter { get; set; }
    public bool IsIncludeCharacter { get; set; }

}

public class RandomPasswordGeneratorResponse
{
    public string ResultPassword { get; set; }
}

[Route("[controller]")]
[ApiController]
public class RandomPasswordController : ControllerBase
{
    [HttpPost("randomPassword")]
    public IActionResult RandomPassword([FromBody] RandomPasswordRequest request)
    {
        string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string Numbers = "1234567890";
        string SpecialCharacters = "!@#$%^&*()-_=+[]{}|;:'";

        string AllowedChars = "";
        StringBuilder Password = new StringBuilder();

        var response = new RandomPasswordGeneratorResponse();

        Random random = new Random();

        if (!request.IsIncludeCharacter && !request.IsIncludeNumbers && !request.IsIncludeLetter)       //!request.IsIncludeCharacter ===>> request.IsIncludeCharacter == false    aynı şey
        {
            ModelState.AddModelError("ValidationError", "En az bir tanesi true olmalıdır.");
            return BadRequest(ModelState);
        }
        if(request.IsIncludeNumbers)    //request.IsIncludeNumbers == true aynı şey
        {
            AllowedChars += Numbers;
        }
        if (request.IsIncludeLetter)
        {
            AllowedChars += Letters;
        }
        if (request.IsIncludeCharacter)
        {
            AllowedChars += SpecialCharacters;
        }

        for(int i=0; i < request.PasswordLength; i++)
        {
            int RandomIndex = random.Next(0,AllowedChars.Length);
            Password.Append(AllowedChars[RandomIndex]);              //Passwordün sonuna ekle
        }
        response.ResultPassword = Password.ToString();
        return Ok(response);
    }
}