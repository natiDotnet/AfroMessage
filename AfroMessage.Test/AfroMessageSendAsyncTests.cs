using RichardSzalay.MockHttp;
using System.Net.Http;
using AfroMessage;
using System.Net;
using System.Threading.Tasks;
using AfroMessage.Test;
using AfroMessage.Results;
using AfroMessage.Responses;
using AfroMessage.DelegatingHandlers;

namespace AfroMessage.Test;

public class AfroMessageSendAsyncTests
{
    private MockHttpMessageHandler mockHttp = new MockHttpMessageHandler();
    
    [Fact]
    public async Task AfroClient_SendAsync_Return401()
    {
        mockHttp.When(HttpMethod.Post, $"{AfroMessageConfig.Url}send")
            .Respond(HttpStatusCode.Unauthorized, new StringContent("Invalid token."));
        var client = new HttpClient(mockHttp)
        {
            BaseAddress = new Uri(AfroMessageConfig.Url)
        };
        var config = new AfroMessageConfig();
        var afro = new AfroMessageClient(config, client);

        var response = await afro.SendMessageAsync("836363883", "Send Message test");
        Assert.True(response.IsFailure);
        Assert.False(response.IsSuccess);
        Assert.Throws<InvalidOperationException>(() => response.Value.ToString());
        Assert.NotEmpty(response.Error.Description);
        Assert.NotEmpty(response.GetErrors());
    }
    [Fact]
    public async Task AfroClient_SendAsync_ReturnErrorResult()
    {
        mockHttp.When(HttpMethod.Post, $"{AfroMessageConfig.Url}send")
            .Respond(HttpStatusCode.OK, new StringContent(
                """
                {
                    "acknowledge": "error",
                    "response": {
                        "errors": [
                            "Either your sender id/name or the identifier(short code) you use are invalid."
                        ],
                        "relatedObject": "/api/send"
                    }
                }
                """));
        var client = new HttpClient(mockHttp)
        {
            BaseAddress = new Uri(AfroMessageConfig.Url)          
        };
        var config = new AfroMessageConfig();
        var afro = new AfroMessageClient(config, client);
        var response = await afro.SendMessageAsync("836363883", "Send Message test");
        
        Assert.True(response.IsFailure);
        Assert.False(response.IsSuccess);
        Assert.Throws<InvalidOperationException>(() => response.Value.ToString());
        Assert.NotEmpty(response.Error.Description);
        Assert.NotEmpty(response.GetErrors());
    }
    [Fact]
    public async Task AfroClient_SendAsync_ReturnSuccessResult()
    {
        mockHttp.When(HttpMethod.Post, $"{AfroMessageConfig.Url}send")
            .Respond(HttpStatusCode.OK, new StringContent(
                """
                    {
                        "acknowledge": "success",
                        "response": {
                            "status": "Send is in progress...",
                            "message_id": "1ef74872-b086-4546-8228-9765351961bb",
                            "message": "Tesfaye Message",
                            "to": "+251912345678"
                        }
                    }
                    """));
        var client = new HttpClient(mockHttp)
        {
            BaseAddress = new Uri(AfroMessageConfig.Url)
        };
        var config = new AfroMessageConfig();
        var afro = new AfroMessageClient(config, client);

        var response = await afro.SendMessageAsync("836363883", "Send Message test");
        Assert.False(response.IsFailure);
        Assert.True(response.IsSuccess);
        Assert.IsType<MessageResponse>(response.Value);
        Assert.Equivalent(response.Value.To, "+251912345678");
        Assert.Empty(response.Error.Description);
        Assert.Empty(response.GetErrors());
    }

}