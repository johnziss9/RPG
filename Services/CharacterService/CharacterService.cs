using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper; 
        }

        private static List<Character> characters = new List<Character>
        {
            new Character(),
            new Character { Id = 1, Name = "Sam" }
        };

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            ServiceResponse<List<GetCharacterDto>> serviceReponse = new ServiceResponse<List<GetCharacterDto>>();

            Character character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(character => character.Id) + 1;
            characters.Add(character);
            serviceReponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
            return serviceReponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> serviceReponse = new ServiceResponse<List<GetCharacterDto>>();

            serviceReponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
            return serviceReponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDto> serviceReponse = new ServiceResponse<GetCharacterDto>();

            serviceReponse.Data = _mapper.Map<GetCharacterDto>(characters.FirstOrDefault(c => c.Id == id));
            return serviceReponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                Character character = characters.FirstOrDefault(c => c.Id == updateCharacter.Id);
                character.Name = updateCharacter.Name;
                character.Class = updateCharacter.Class;
                character.Defense = updateCharacter.Defense;
                character.HitPoints = updateCharacter.HitPoints;
                character.Intelligence = updateCharacter.Intelligence;
                character.Strength = updateCharacter.Strength;

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                Character character = characters.First(c => c.Id == id);
                characters.Remove(character);

                serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}