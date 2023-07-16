using AutoMapper;
using CustomerInfo.Dto;
using CustomerInfo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerInfo
{
	public class CustomerInfoAutoMapperProfile : Profile
	{
		public CustomerInfoAutoMapperProfile()
		{
			//CreateMap<CreateCustomerDto, CustomerDto>()
			//.ReverseMap()
			//.IgnoreAllPropertiesWithAnInaccessibleSetter();

			//CreateMap<CreateCustomerDto, Customer>()
			//.ReverseMap()
			//.IgnoreAllPropertiesWithAnInaccessibleSetter();

			CreateMap<CustomerDto, Customer>()
			.ReverseMap()
			.IgnoreAllPropertiesWithAnInaccessibleSetter();
		}
	}
}
