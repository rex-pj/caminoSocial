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
import farmCreationModel from "../../../models/farmCreationModel";
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
    currentFarm,
  } = props;
  const [formData, setFormData] = useState(
    JSON.parse(JSON.stringify(farmCreationModel))
  );
  const editorRef = useRef();
  const selectRef = useRef();

  const handleInputChange = (evt) => {
    let data = formData || {};
    const { name, value } = evt.target;

    data[name].isValid = checkValidity(data, value, name);
    data[name].value = value;

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
    let data = { ...formData } || {};
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

  const onFarmPost = async (e) => {
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

    const farmData = {};
    for (const formIdentifier in formData) {
      farmData[formIdentifier] = formData[formIdentifier].value;
    }

    await props.onFarmPost(farmData).then((response) => {
      if (response && response.id) {
        clearFormData();
      }
    });
  };

  const loadFarmTypeSelections = (value, callback) => {
    return filterCategories(value).then((response) => {
      return response;
    });
  };

  const handleSelectChange = (e, method) => {
    const { action } = method;
    let data = formData || {};
    if (action === "clear" || action === "remove-value") {
      data.farmTypeId.isValid = false;
      data.farmTypeId.value = 0;
      data.farmTypeName.value = "";
    } else {
      const { value, label } = e;
      data.farmTypeId.isValid = checkValidity(data, value, "farmTypeId");
      data.farmTypeId.value = parseFloat(value);
      data.farmTypeName.value = label;
    }

    setFormData({
      ...data,
    });
  };

  const loadFarmTypeSelected = () => {
    const { farmTypeId, farmTypeName } = formData;
    if (!farmTypeId.value) {
      return null;
    }

    return {
      label: farmTypeName.value,
      value: farmTypeId.value,
    };
  };

  const clearFormData = () => {
    editorRef.current.clearEditor();
    selectRef.current.select.select.clearValue();
    setFormData(JSON.parse(JSON.stringify(farmCreationModel)));
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

  useEffect(() => {
    if (currentFarm && !formData?.id?.value) {
      setFormData(currentFarm);
    }
  }, [currentFarm, formData]);

  const { name, address, farmTypeId, thumbnails } = formData;
  return (
    <Fragment>
      <form onSubmit={(e) => onFarmPost(e)} method="POST">
        <FormRow className="row">
          <div className="col-6 col-lg-6 pr-lg-1">
            <PrimaryTextbox
              name="name"
              value={name.value}
              autoComplete="off"
              onChange={(e) => handleInputChange(e)}
              placeholder="Farm title"
            />
          </div>
          <div className="col-6 col-lg-6 pl-lg-1">
            <AsyncSelect
              key={JSON.stringify(farmTypeId)}
              className="cate-selection"
              cacheOptions
              defaultOptions
              ref={selectRef}
              defaultValue={loadFarmTypeSelected()}
              onChange={(e, action) => handleSelectChange(e, action)}
              loadOptions={loadFarmTypeSelections}
              isClearable={true}
            />
          </div>
        </FormRow>
        <FormRow className="row">
          <div className="col-9 col-lg-10 pr-lg-1">
            <PrimaryTextbox
              name="address"
              value={address.value ? address.value : ""}
              autoComplete="off"
              onChange={(e) => handleInputChange(e)}
              placeholder="Address"
            />
          </div>
          <div className="col-3 col-lg-2 pl-lg-1 pl-1">
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
          contentHtml={currentFarm ? currentFarm.description.value : null}
          height={height}
          convertImageCallback={convertImageCallback}
          onImageValidate={onImageValidate}
          placeholder="Enter the description here"
          onChanged={onDescriptionChanged}
          ref={editorRef}
        />
        <Footer className="row mb-3">
          <div className="col-auto"></div>
          <div className="col-auto ms-auto">
            <ButtonPrimary size="xs">Post</ButtonPrimary>
          </div>
        </Footer>
      </form>
    </Fragment>
  );
});
