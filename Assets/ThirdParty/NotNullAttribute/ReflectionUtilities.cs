﻿using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace RedBlue.Extensions
{
	public static class ReflectionUtilities
	{
		public static List<FieldInfo> GetFieldsWithAttributeFromType<T> (Type classToInspect, 
	                                                                      BindingFlags reflectionFlags = BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Instance)
		{
			List<FieldInfo> fieldsWithAttribute = new List<FieldInfo> ();
			FieldInfo[] allFields;
			if (reflectionFlags == BindingFlags.Default) {
				allFields = classToInspect.GetFields (reflectionFlags);
			} else {
				allFields = classToInspect.GetFields (reflectionFlags);
			}
			foreach (FieldInfo fieldInfo in allFields) {
				foreach (Attribute attribute in Attribute.GetCustomAttributes (fieldInfo)) {
					if (attribute.GetType () == typeof(T)) {
						fieldsWithAttribute.Add (fieldInfo);
						break;
					}
				}
			}

			return fieldsWithAttribute;
		}
	}
}