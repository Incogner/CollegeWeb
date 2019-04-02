using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using College.Infrastructure;

namespace College.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;
            SessionCart Cart = session?.GetJson<SessionCart>("Cart")
                ?? new SessionCart();
            Cart.Session = session;
            return Cart;
        }

        [JsonIgnore]
        public ISession Session { get; set; }

        public override void AddCourse(Course course)
        {
            base.AddCourse(course);
            Session.SetJson("Cart", this);
        }

        public override void RemoveCourse(Course course)
        {
            base.RemoveCourse(course);
            Session.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}
