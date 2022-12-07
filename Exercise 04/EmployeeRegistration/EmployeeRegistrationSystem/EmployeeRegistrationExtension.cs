
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using EmployeeRegistrationSystem.BLL;
using EmployeeRegistrationSystem.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
#endregion

namespace EmployeeRegistrationSystem
{

    //your class needs to be public so it can be used outside
    //  of this project
    //this class also needs to be static
    public static class EmployeeRegistrationExtension
    {
        //method name can be anything, it must match
        //  the match the builder.Services.xxxxx(options => ..
        //  statement in your Program.cs

        //the first parameter is the class that you are attempting
        //  to extend

        //the second parameter is the options value in your
        //  call statement
        //it is receiving the connectionstring for your application

        public static void EmployeeSystemBackendDependencies(
            this IServiceCollection services,
            Action<DbContextOptionsBuilder> options
            )
        {
            //register the DbContext class with the service collection
            services.AddDbContext<WorkScheduleContext>(options);

            //add any services that you create in the class library
            //  using .AddTrainsient<serviceclassname>(....);

            services.AddTransient<SkillServices>((serviceProvider) =>
            {
                //retrieve the registered DcContext done with
                //  AddDbContext
                var context = serviceProvider.GetRequiredService<WorkScheduleContext>();
                return new SkillServices(context);
            });

            services.AddTransient<FetchEmployeeSkillServices>((serviceProvider) =>
            {
                //retrieve the registered DcContext done with
                //  AddDbContext
                var context = serviceProvider.GetRequiredService<WorkScheduleContext>();
                return new FetchEmployeeSkillServices(context);
            });

        }

    }
}
