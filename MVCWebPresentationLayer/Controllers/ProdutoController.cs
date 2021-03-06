﻿using AutoMapper;
using BLL.Impl;
using Common;
using DTO;
using MVCWebPresentationLayer.Mock;
using MVCWebPresentationLayer.Models;
using MVCWebPresentationLayer.Models.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVCWebPresentationLayer.Controllers
{
    public class ProdutoController : Controller
    {
        FornecedorService fsvc = new FornecedorService();
        CategoriaService csvc = new CategoriaService();

        public async Task<ActionResult> Cadastrar()
        {
            List<FornecedorDTO> fornecedores = await fsvc.GetFornecedores();
            List<CategoriaDTO> categorias = await csvc.GetCategorias();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FornecedorDTO, FornecedorQueryViewModel>();
                cfg.CreateMap<CategoriaDTO, CategoriaQueryViewModel>();
            });

            IMapper mapper = configuration.CreateMapper();
            // new SERService().GetSERByID(4);
            //Transforma o ClienteInsertViewModel em um ClienteDTO

            //Este objeto "dados" é uma lista de objetos ViewModel
            List<FornecedorQueryViewModel> dadosFornecedores = mapper.Map<List<FornecedorQueryViewModel>>(fornecedores);
            List<CategoriaQueryViewModel> dadosCategorias = mapper.Map<List<CategoriaQueryViewModel>>(categorias);

            ViewBag.Fornecedores = dadosFornecedores;
            ViewBag.Categorias = dadosCategorias;

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Cadastrar(ProdutoViewModel viewModel)
        {
            //Estas 3 linhas utilizarão o AutoMapper pra transformar um ProdutoViewModel em um ProdutoDTO
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProdutoViewModel, ProdutoDTO>();
            });
            IMapper mapper = configuration.CreateMapper();
            ProdutoDTO produto = mapper.Map<ProdutoDTO>(viewModel);

            ProdutoService svc = new ProdutoService();
            try
            {
                await svc.Insert(produto);
                return RedirectToAction("Index", "Produto");
            }
            catch (NecoException ex)
            {
                ViewBag.Errors = ex.Errors;
            }
            catch(Exception ex)
            {
                ViewBag.ErroGenerico = ex.Message;
            }
            return View();
        }



        // GET: Produto
        public ActionResult Index()
        {
            return View();
        }
    }
}