﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Services;

namespace SIEL_1836109025062022.Controllers
{
    public class LevelsController : Controller 
    {
        private readonly ICourseProgramRepository courseProgramRepository;
        private readonly ILevelsRepository levelsRepository;

        public LevelsController(
            ICourseProgramRepository courseProgramRepository,
            ILevelsRepository levelsRepository)
        {
            this.courseProgramRepository = courseProgramRepository;
            this.levelsRepository = levelsRepository;
        }

        public async Task<IActionResult> Index()
        {
            var levels = await levelsRepository.GetLevels();

            var modelo = levels
                .GroupBy(x => x.program_name)
                .Select(grupo => new IndexLevelsViewModel
                {
                    program = grupo.Key,
                    levels= grupo.AsEnumerable()
                }).ToList();
            return View(modelo);
        }

        [HttpGet]
        public  async Task<IActionResult> CreateLevel()
        {
            
            var modelo = new LevelCreateViewModel();
            modelo.Programs = await GetAllCoursePrograms();
            return View(modelo);  
        }

        [HttpPost]
        public async Task<IActionResult> CreateLevel(LevelCreateViewModel level)
        {
            var program = await courseProgramRepository.GetCourseProgramById(level.level_id_program);
            if(program is null)
            {
                return RedirectToAction("Errore", "Home");
            }

            if (!ModelState.IsValid)
            {
                level.Programs = await GetAllCoursePrograms();
                return View(level);
            }

            await levelsRepository.CreateLevel(level);
            return RedirectToAction("index");
        }

        private async Task<IEnumerable<SelectListItem>> GetAllCoursePrograms()
        {
            var programs = await courseProgramRepository.GetAllCoursePrograms();
            return programs.Select(x => new SelectListItem(
                    x.program_name,
                    x.id_program.ToString()));
        }

        [HttpGet]
        public async Task<IActionResult> EditLevel(int id)
        {
            var level = await levelsRepository.GetLevelById(id);
            if (level is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            return View(level);
        }

        [HttpPost]
        public async Task<ActionResult> EditLevel(Level level)
        {
            var levelExists = await levelsRepository.GetLevelById(level.id_level);

            if (levelExists is null)
            {
                return RedirectToAction("Errore", "Home");
            }
            await levelsRepository.UpdateLevel(level);
            return RedirectToAction("Index");


        }
    }
}
