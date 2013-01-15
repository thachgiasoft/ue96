using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Drawing;

using Icson.Objects;
using Icson.Objects.Online;

namespace YoeJoyHelper
{
    public class CommonUtility
    {
        /// <summary>
        /// 保存图片的公共方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="uploader"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static string SaveImage(HttpContext context, FileUpload uploader, out string Msg)
        {
            Msg = String.Empty;
            string newImgPath = String.Empty;
            try
            {
                //直接取得文件名
                string fileName = uploader.FileName.ToString();
                //取得上传文件路径
                string url = uploader.PostedFile.FileName.ToString();
                //获取文件MIME内容类型
                string type = uploader.PostedFile.ContentType;
                string type2 = fileName.Substring(fileName.IndexOf(".") + 1);
                //获取文件大小
                int size = uploader.PostedFile.ContentLength;

                //判断同名文件
                if (File.Exists(url))
                {
                    Msg = "存在同名文件，请重新上传";
                }
                else
                {
                    if (type2 == "gif" || type2 == "jpg" || type2 == "bmp" || type2 == "png")
                    {
                        if (size <= 4134904)
                        {
                            //string basePath = context.Server.MapPath(imgFolderPath) + "\\";
                            string basePath = String.Concat(ConfigurationManager.AppSettings["ImagePhyicalPath"].ToString(), "products\\");
                            string newFileName = String.Concat(Guid.NewGuid(), fileName);
                            uploader.SaveAs(String.Concat(basePath, newFileName));
                            Msg = "保存成功！";
                            newImgPath = newFileName;
                        }
                        else
                        {
                            Msg = "文件大于4M，请重新上传";
                        }
                    }
                }
                return newImgPath;
            }
            catch
            {
                Msg = "Server Error: 保存图片失败";
                return newImgPath;
            }
        }

        /// <summary>
        /// 保存商品图片的公共方法
        /// 后台上传商品图片，会自动噶呢句输入的缩略图尺寸
        /// 生成相应的缩略图
        /// </summary>
        /// <param name="context"></param>
        /// <param name="uploader"></param>
        /// <param name="resizedWidth"></param>
        /// <param name="resizedHeight"></param>
        /// <param name="Msg"></param>
        /// <returns></returns>
        public static ProductImage SaveProductImg(HttpContext context, FileUpload uploader, int resizedWidth, out string Msg)
        {
            Msg = String.Empty;
            string newImageFile = String.Empty;
            string newResizedImageFile = String.Empty;
            ProductImage image = new ProductImage();
            try
            {
                //直接取得文件名
                string fileName = uploader.FileName.ToString();
                //取得上传文件路径
                string url = uploader.PostedFile.FileName.ToString();
                //获取文件MIME内容类型
                string type = uploader.PostedFile.ContentType;
                string type2 = fileName.Substring(fileName.IndexOf(".") + 1);
                //获取文件大小
                int size = uploader.PostedFile.ContentLength;

                //判断同名文件
                if (File.Exists(url))
                {
                    Msg = "存在同名文件，请修改文件名重新上传";
                }
                else
                {
                    if (type2 == "gif" || type2 == "jpg" || type2 == "bmp" || type2 == "png")
                    {
                        if (size <= 4134904)
                        {
                            string basePath = String.Concat(YoeJoyConfig.ImgPhysicalPathBase, "products\\");
                            string newFileName = String.Concat(Guid.NewGuid(), fileName);
                            //保存原始图片
                            uploader.SaveAs(String.Concat(basePath, newFileName));
                            newImageFile = newFileName;
                            //保存缩略图片
                            using (System.Drawing.Image imageObj = System.Drawing.Image.FromStream(uploader.FileContent, true))
                            {
                                int originalImageWidth = imageObj.Width;
                                string thumbnailImageFileName = String.Concat("thumbnail_", newFileName);
                                string thumbnailImagePath = String.Concat(YoeJoyConfig.ImgPhysicalPathBase, "products\\", thumbnailImageFileName);

                                //当原始图片的宽度小于设定缩小的宽度，则直接保存该文件
                                if (resizedWidth >= originalImageWidth)
                                {
                                    uploader.SaveAs(thumbnailImagePath);
                                }
                                else
                                {
                                    //当原始图片的宽度大于需要缩小的宽度则等比例缩放

                                    int originalImageHeight = imageObj.Height;

                                    int resizedHeight = (resizedWidth * originalImageHeight) / originalImageWidth;

                                    using (System.Drawing.Image thumbnailImg = new Bitmap(resizedWidth, resizedHeight))
                                    {
                                        using (Graphics thumbnailGraphic = Graphics.FromImage(thumbnailImg))
                                        {
                                            //绘制缩略图
                                            thumbnailGraphic.DrawImage(imageObj, new Rectangle(0, 0, resizedWidth, resizedHeight), new Rectangle(0, 0, originalImageWidth, originalImageHeight), GraphicsUnit.Pixel);

                                            //保存缩略图
                                            thumbnailImg.Save(thumbnailImagePath);
                                        }
                                    }
                                }
                                newResizedImageFile = thumbnailImageFileName;
                            }
                            image.LargeImg = newImageFile;
                            image.SmallImg = newResizedImageFile;
                            Msg = "保存成功！";
                            return image;
                        }
                        else
                        {
                            Msg = "文件大于4M，请重新上传";
                            image = null;
                        }
                    }
                }
                return image;
            }
            catch
            {
                Msg = "Server Error: 保存图片失败";
                return null;
            }
        }

        /// <summary>
        /// 绑定下拉列表初始的状态
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> BindTrueFalseStatus()
        {
            Dictionary<int, string> statusDic = new Dictionary<int, string>();
            statusDic.Add(0, "无效");
            statusDic.Add(1, "有效");
            return statusDic;
        }

        public static Dictionary<int, string> BindTrueFalseStatus(int status)
        {
            Dictionary<int, string> statusDic = new Dictionary<int, string>();
            if (status == 0)
            {
                statusDic.Add(status, "无效");
                statusDic.Add(1, "有效");
            }
            else
            {
                statusDic.Add(status, "有效");
                statusDic.Add(0, "无效");
            }
            return statusDic;
        }

        /// <summary>
        /// 通用正则
        /// </summary>
        /// <param name="num">待验证信息</param>
        /// <param name="match">正则表达式</param>
        /// <returns>通过与否 true or false</returns>
        public static bool IsValidNum(string num, string match)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(num, match);
        }

        /// <summary>
        /// 获得用户的session
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IcsonSessionInfo GetUserSession(HttpContext context)
        {
            IcsonSessionInfo oSession = (IcsonSessionInfo)context.Session["IcsonSessionInfo"];
            if (oSession == null)
            {
                oSession = new IcsonSessionInfo();
                context.Session["IcsonSessionInfo"] = oSession;
            }
            return oSession;
        }

    }
}