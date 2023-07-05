export function createFormData(files: File[], storageId: string) : FormData{
    //handle upload + progress
    const formData = new FormData();
    
    if(files.length <= 0) return formData;

    for (let i = 0; i < files.length; i++) {
      formData.append('files', files[i], files[i].name);
    }

    formData.append('storageId', storageId);

    return formData;
}