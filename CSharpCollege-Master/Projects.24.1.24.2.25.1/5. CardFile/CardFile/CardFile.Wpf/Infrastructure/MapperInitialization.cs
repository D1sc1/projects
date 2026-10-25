using AutoMapper;
using CardFile.Business.Models;
using CardFile.Common.Infrastructure;
using CardFile.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.Wpf.Infrastructure
{
    internal static class MapperInitialization
    {
        public static void PreRegister()
        {
            Mapping.InitializationAction += Configure;
        }

        private static void Configure(IMapperConfigurationExpression cfg)
        {
            // Courses - список для выбора в окне правки, данных в нём нет
            cfg.CreateMap<Student, StudentViewModel>()
                .ForMember(d => d.Courses, o => o.Ignore());
            cfg.CreateMap<StudentViewModel, Student>();
            cfg.CreateMap<StudentViewModel, StudentViewModel>()
                .ForMember(d => d.Courses, o => o.Ignore());
        }
    }
}
