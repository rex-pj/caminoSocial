import React, { Fragment, useState, useRef, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { withRouter } from "react-router-dom";
import CommonEditor from "../CommonEditor";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { checkValidity } from "../../../utils/Validity";
import styled from "styled-components";
import { stateToHTML } from "draft-js-export-html";
import ImageUpload from "../UploadControl/ImageUpload";
import AsyncSelect from "react-select/async";
import productCreationModel from "../../../models/productCreationModel";
import { Thumbnail } from "../../molecules/Thumbnails";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};

  ${PrimaryTextbox} {
    max-width: 100%;
    width: 100%;
  }

  .cate-selection {
    z-index: 10;

    > div {
      border: 1px solid ${(p) => p.theme.color.primaryDivide};
    }
  }

  ${AsyncSelect} {
    max-width: 100%;
  }
`;

const ThumbnailUpload = styled(ImageUpload)`
  text-align: center;
  margin: auto;
  display: inline-block;
  vertical-align: middle;

  > span {
    color: ${(p) => p.theme.color.primaryText};
    height: ${(p) => p.theme.size.normal};
    padding: 0 ${(p) => p.theme.size.tiny};
    font-size: ${(p) => p.theme.fontSize.tiny};
    background-color: ${(p) => p.theme.color.lightBg};
    border-radius: ${(p) => p.theme.borderRadius.normal};
    border: 1px solid ${(p) => p.theme.color.neutralBg};
    cursor: pointer;
    font-weight: 600;

    :hover {
      background-color: ${(p) => p.theme.color.neutralBg};
    }

    svg {
      display: inline-block;
      margin: 10px auto 0 auto;
    }
  }
`;

const ImageEditBox = styled.div`
  position: relative;
`;

const RemoveImageButton = styled.span`
  position: absolute;
  top: -${(p) => p.theme.size.exSmall};
  right: -${(p) => p.theme.size.exTiny};
  cursor: pointer;
`;

const Footer = styled.div`
  ${ButtonPrimary} {
    width: 200px;
  }
`;

export default withRouter((props) => {
  const {
    convertImageCallback,
    onImageValidate,
    height,
    filterCategories,
    filterFarms,
    currentProduct,
  } = props;
  const [formData, setFormData] = useState(
    JSON.parse(JSON.stringify(productCreationModel))
  );
  const editorRef = useRef();
  const categorySelectRef = useRef();
  const farmSelectRef = useRef();

  const handleInputChange = (evt) => {
    let data = formData || {};
    const { name, value } = evt.target;

    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

    setFormData({
      ...data,
    });
  };

  const handlePriceChange = (evt) => {
    let data = formData || {};
    const { value } = evt.target;
    const name = "price";

    data[name].isValid = checkValidity(data, value, name);
    data[name].value = parseInt(value);

    setFormData({
      ...data,
    });
  };

  const onDescriptionChanged = (editorState) => {
    const contentState = editorState.getCurrentContent();
    const html = stateToHTML(contentState);

    let data = formData || {};

    data["description"].isValid = checkValidity(data, html, "description");
    data["description"].value = html;

    setFormData({
      ...data,
    });
  };

  const handleImageChange = (e) => {
    let data = formData || {};
    const { preview, file } = e;
    const { name, type } = file;

    let thumbnails = Object.assign([], data.thumbnails.value);
    thumbnails.push({
      base64Data: preview,
      fileName: name,
      contentType: type,
    });

    data.thumbnails.value = thumbnails;
    setFormData({
      ...data,
    });
  };

  const onProductPost = async (e) => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;
      break;
    }

    if (!isFormValid) {
      props.showValidationError(
        "Something went wrong with your input",
        "Something went wrong with your information, please check and input again"
      );

      return;
    }

    const productData = {};
    for (const formIdentifier in formData) {
      productData[formIdentifier] = formData[formIdentifier].value;
    }

    await props.onProductPost(productData).then((response) => {
      if (response && response.id) {
        clearFormData();
      }
    });
  };

  const clearFormData = () => {
    editorRef.current.clearEditor();
    categorySelectRef.current.select.select.clearValue();
    farmSelectRef.current.select.select.clearValue();
    var productFormData = JSON.parse(JSON.stringify(productCreationModel));
    setFormData({ ...productFormData });
  };

  const onImageRemoved = (e, item) => {
    let data = formData || {};
    if (!data.thumbnails) {
      return;
    }

    if (item.pictureId) {
      data.thumbnails.value = data.thumbnails.value.filter(
        (x) => x.pictureId !== item.pictureId
      );
    } else {
      data.thumbnails.value = data.thumbnails.value.filter((x) => x !== item);
    }

    setFormData({
      ...data,
    });
  };

  const handleSelectChange = async (e, method, name) => {
    let data = formData || {};
    const { action, removedValue } = method;
    if (action === "clear") {
      data[name].value = [];
      data[name].isValid = false;
    } else if (action === "remove-value") {
      data[name].value = data[name].value.filter(
        (x) => x.id !== parseFloat(removedValue.value)
      );
      data[name].isValid = !!data[name].value;
    } else {
      data[name].value = e.map((item) => {
        return { id: parseFloat(item.value), name: item.label };
      });

      data[name].isValid = data[name].value && data[name].value.length > 0;
    }

    setFormData({
      ...data,
    });
  };

  const loadCategoriesSelected = () => {
    const { categories } = formData;
    if (!categories.value) {
      return null;
    }
    return categories.value.map((item) => {
      return { value: item.id, label: item.name };
    });
  };

  const loadCategorySelections = (value, callback) => {
    const { categories } = formData;
    var currentIds = categories?.value?.map((cate) => cate.id);
    return filterCategories(value, currentIds).then((response) => {
      return response;
    });
  };

  const loadFarmsSelected = () => {
    const { farms } = formData;
    if (!farms.value) {
      return null;
    }
    return farms.value.map((item) => {
      return { value: item.id, label: item.name };
    });
  };

  const loadFarmSelections = (value, callback) => {
    const { farms } = formData;
    const currentIds = farms?.value?.map((farm) => farm.id);
    return filterFarms(value, currentIds).then((response) => {
      return response;
    });
  };

  useEffect(() => {
    if (currentProduct && !formData?.id?.value) {
      setFormData(currentProduct);
    }
  }, [currentProduct, formData]);

  const { name, price, thumbnails, categories, farms } = formData;

  return (
    <Fragment>
      <form onSubmit={(e) => onProductPost(e)} method="POST">
        <FormRow className="row">
          <div className="col-9 col-lg-9 pr-1">
            <PrimaryTextbox
              name="name"
              value={name.value}
              autoComplete="off"
              onChange={(e) => handleInputChange(e)}
              placeholder="Product title"
            />
          </div>
          <div className="col-3 col-lg-3 pl-1">
            <PrimaryTextbox
              name="price"
              value={price.value}
              autoComplete="off"
              onChange={(e) => handlePriceChange(e)}
              placeholder="Price"
            />
          </div>
        </FormRow>
        <FormRow className="row">
          <div className="col-4 col-lg-5 pr-1">
            <AsyncSelect
              key={JSON.stringify(categories)}
              className="cate-selection"
              defaultOptions
              isMulti
              ref={categorySelectRef}
              defaultValue={loadCategoriesSelected()}
              onChange={(e, action) =>
                handleSelectChange(e, action, "categories")
              }
              loadOptions={loadCategorySelections}
              isClearable={true}
              cache={false}
              placeholder="Select categories"
            />
          </div>
          <div className="col-4 col-lg-5 pr-1">
            <AsyncSelect
              className="cate-selection"
              key={JSON.stringify(farms)}
              defaultOptions
              isMulti
              ref={farmSelectRef}
              defaultValue={loadFarmsSelected()}
              onChange={(e, action) => handleSelectChange(e, action, "farms")}
              loadOptions={loadFarmSelections}
              isClearable={true}
              placeholder="Select farms"
            />
          </div>
          <div className="col-4 col-lg-2 pl-1">
            <ThumbnailUpload onChange={handleImageChange}></ThumbnailUpload>
          </div>
        </FormRow>
        {thumbnails.value ? (
          <FormRow className="row">
            {thumbnails.value.map((item, index) => {
              if (item.base64Data) {
                return (
                  <div className="col-3" key={index}>
                    <ImageEditBox>
                      <Thumbnail src={item.base64Data}></Thumbnail>
                      <RemoveImageButton
                        onClick={(e) => onImageRemoved(e, item)}
                      >
                        <FontAwesomeIcon icon="times"></FontAwesomeIcon>
                      </RemoveImageButton>
                    </ImageEditBox>
                  </div>
                );
              } else if (item.pictureId) {
                return (
                  <div className="col-3" key={index}>
                    <ImageEditBox>
                      <Thumbnail
                        src={`${process.env.REACT_APP_CDN_PHOTO_URL}${item.pictureId}`}
                      ></Thumbnail>
                      <RemoveImageButton
                        onClick={(e) => onImageRemoved(e, item)}
                      >
                        <FontAwesomeIcon icon="times"></FontAwesomeIcon>
                      </RemoveImageButton>
                    </ImageEditBox>
                  </div>
                );
              }

              return null;
            })}
          </FormRow>
        ) : null}
        <CommonEditor
          contentHtml={currentProduct ? currentProduct.description.value : null}
          height={height}
          convertImageCallback={convertImageCallback}
          onImageValidate={onImageValidate}
          placeholder="Enter the description here"
          onChanged={onDescriptionChanged}
          ref={editorRef}
        />
        <Footer className="row mb-3">
          <div className="col-auto"></div>
          <div className="col-auto ml-auto">
            <ButtonPrimary size="xs">Post</ButtonPrimary>
          </div>
        </Footer>
      </form>
    </Fragment>
  );
});
