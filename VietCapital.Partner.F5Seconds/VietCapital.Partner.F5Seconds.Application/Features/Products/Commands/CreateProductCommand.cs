// using AutoMapper;
// using MediatR;
// using System.Collections.Generic;
// using System.ComponentModel;
// using System.Threading;
// using System.Threading.Tasks;
// using VietCapital.Partner.F5Seconds.Application.DTOs.Gateway;
// using VietCapital.Partner.F5Seconds.Application.Interfaces;
// using VietCapital.Partner.F5Seconds.Application.Wrappers;

// namespace VietCapital.Partner.F5Seconds.Application.Features.Categories.Commands
// {
//     public class CreateProductCommand : IRequest<Response<List<F5sVoucherCode>>>
//     {
//         public string Name { get; set; }
//         public string Image { get; set; }
//         public bool Status { get; set; }
//         public List<CategoryProduct> CategoryProducts { get; set; }
//         public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response<List<F5sVoucherCode>>>
//         {
//             private readonly ICategoryRepositoryAsync _categoryRepositoryAsync;
//             private readonly IMapper _mapper;
//             public CreateProductCommandHandler(ICategoryRepositoryAsync categoryRepositoryAsync, IMapper mapper)
//             {
//                 _categoryRepositoryAsync = categoryRepositoryAsync;
//                 _mapper = mapper;
//             }

//             public async Task<Response<List<F5sVoucherCode>>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
//             {
               
//             }
//         }
//     }
// }
