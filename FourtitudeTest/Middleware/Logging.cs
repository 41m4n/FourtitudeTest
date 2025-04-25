using log4net;

namespace FourtitudeTest.Middleware
{
    public class Logging
    {
        private readonly RequestDelegate _next;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Logging));

        public Logging(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0;

            _logger.Info($"[Request] {context.Request.Method} {context.Request.Path} - Body: {requestBody}");

            var originalBodyStream = context.Response.Body;
            using var newBodyStream = new MemoryStream();
            context.Response.Body = newBodyStream;

            try
            {
                await _next(context);

                newBodyStream.Position = 0;
                var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();
                newBodyStream.Position = 0;

                _logger.Info($"[Response] {context.Response.StatusCode} - Body: {responseBody}");

                await newBodyStream.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                _logger.Error("Unhandled exception:", ex);
                throw;
            }
        }
    }
}
