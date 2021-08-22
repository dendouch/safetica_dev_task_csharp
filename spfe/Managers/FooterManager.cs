using spfe.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace spfe.Managers
{
    public class FooterManager
    {
        internal static void ExecuteCommand(Command cmd, string filename, string arg)
        {
            if (cmd == Command.UNDEFINED) throw new IOEditorException("Invalid command.");

            //Load footer and preprocess
            KeyValuePair<int, string> footerSource = new KeyValuePair<int, string>();
            try
            {
                footerSource = FileManager.GetFooterStrWithIdxFromFile(filename);
            } catch (Exception ex)
            {
                throw new IOEditorException($"Unable to load the footer:\n{ex.Message}");
            }

            var newFooter = ProcessFooter(cmd, footerSource.Value, arg);            

            //Write to file, update/create footer
            try
            {
                FileManager.WriteFooterToFile(newFooter, filename, footerSource.Value.Length);
            }
            catch (Exception ex)
            {
                throw new IOEditorException($"Unable to save the footer:\n{ex.Message}");
            }
        }

        private static string ProcessFooter(Command command, string footerSource, string arg)
        {
            bool hasFooter = !footerSource.Equals(String.Empty);
            string result;
            switch (command)
            {
                case (Command.ADD):
                    result = ExtendFooterValues(hasFooter,
                        arg.Split(Constants.PropertyNameValueDelimeter)[0],
                        arg.Split(Constants.PropertyNameValueDelimeter)[1],
                        footerSource);
                    break;
                case (Command.EDIT):
                    result = UpdateFooterValue(hasFooter,
                        arg.Split(Constants.PropertyNameValueDelimeter)[0],
                        arg.Split(Constants.PropertyNameValueDelimeter)[1],
                        footerSource);
                    break;
                case (Command.REMOVE):
                    result = ExtractFooterValue(hasFooter, arg, footerSource);
                    break;
                default:
                    return String.Empty;
            }
            Console.WriteLine($"Command - {command.ToString()} has been successfully executed.");
            return result;
        }

        private static string ExtractFooterValue(bool hasFooter, string propertyName, string footer)
        {
            if (!hasFooter) throw new IOEditorException("File does not contain footer.");
            else
            {
                if (!IsPropertyInFooter(propertyName, footer)) throw new IOEditorException("Property is not present in the footer.");
                else
                {
                    //match by prop name and get everything until first \n
                    var oldProperty = Regex.Match(footer, $@"{propertyName}=([^\\\n]*)"); 
                    var oldValue = oldProperty.Groups[1].Value;
                    var sb = new StringBuilder(footer);
                    sb.Remove(oldProperty.Index - 2, propertyName.Length + 1 + oldValue.Length + 2);
                    return sb.ToString();
                }
            }
        }

        private static string UpdateFooterValue(bool hasFooter, string newPropertyName, string newPropertyValue, string footer)
        {
            if (newPropertyValue.Equals(String.Empty)) throw new IOEditorException("Please specify the value of the new property.");
            if (!hasFooter) throw new IOEditorException("File does not contain footer.");
            else
            {
                if (!IsPropertyInFooter(newPropertyName, footer)) throw new IOEditorException("Property is not present in the footer.");
                else
                {
                    //match by prop name and get everything until first \n
                    var oldProperty = Regex.Match(footer, $@"{newPropertyName}=([^\\\n]*)");
                    var oldValueIdx = oldProperty.Index + newPropertyName.Length + 1;
                    var oldValue = oldProperty.Groups[1].Value;
                    var propLengthDiff = oldValue.Length
                        < newPropertyValue.Length
                        ? newPropertyValue.Length - oldValue.Length
                        : oldValue.Length - newPropertyValue.Length;
                    if (footer.Length + propLengthDiff > Constants.FooterMaxLength)
                        throw new IOEditorException($"Can not {Command.UNDEFINED.ToString()} new property, footer will be too long.");
                    var sb = new StringBuilder(footer);
                    sb.Remove(oldValueIdx, oldValue.Length);
                    sb.Insert(oldValueIdx, newPropertyValue);
                    return sb.ToString();
                }
            }
        }

        private static string ExtendFooterValues(bool hasFooter, string newPropertyName, string newPropertyValue, string footer)
        {
            string result;
            if (newPropertyValue.Equals(String.Empty)) throw new IOEditorException("Please specify the value of the new property.");
            if (hasFooter)
            {
                if (!IsPropertyInFooter(newPropertyName, footer))
                {
                    if (footer.Length 
                        + newPropertyName.Length
                        + 1
                        + newPropertyValue.Length 
                        + 2 
                        > Constants.FooterMaxLength) throw new IOEditorException($"Can not {Command.ADD.ToString()} new property, footer will be too long.");
                    footer += $"\\n{newPropertyName}{Constants.PropertyNameValueDelimeter}{newPropertyValue}";
                    result = footer;
                }
                else throw new IOEditorException("File already contains footer with this property."); 
            }
            else
            {
                result = $"{Constants.FooterHeaderCorrectFormat}\\n{newPropertyName}={newPropertyValue}";
            }
            return result;
        }

        private static bool IsPropertyInFooter(string propName, string newFooter)
        {
            //adding the '=' at the end so it will exclude all the subsets of propName
            return Regex.IsMatch(newFooter, propName + Constants.PropertyNameValueDelimeter);
        }
    }
}
