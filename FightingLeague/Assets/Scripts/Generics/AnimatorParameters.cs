using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterControl
{
	public class AnimatorParameters
	{

		private List<AnimatorControllerParameter> parameters;

		public AnimatorParameters(List<AnimatorControllerParameter> parameters)
		{
			this.parameters = parameters;
		}

		public List<AnimatorControllerParameter> FindAnimatorParameter(String[] names)
		{
			List<AnimatorControllerParameter> list = new List<AnimatorControllerParameter>();
			AnimatorControllerParameter parametro;

			foreach (String name in names)
			{
				parametro = parameters.Find(parameter => parameter.name == name);
				list.Add(parametro);
			}

			return list;
		}
	}

}