import React, { Fragment, useState } from "react";
import { PanelBody, PanelFooter } from "../../atoms/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import styled from "styled-components";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};
  .cate-selection {
    > div {
      border: 1px solid ${(p) => p.theme.color.primaryDivide};
    }
  }

  ${PrimaryTextbox} {
    width: 100%;
  }
`;

export default function (props) {
  const { data, execution } = props;
  const { onEditAttributeValue } = execution;
  const { attributeValue, index } = data;
  const [formData, setFormData] = useState(attributeValue);

  const handleInputChange = (evt) => {
    let attribute = formData;
    const { name, value } = evt.target;
    attribute[name] = value;

    setFormData({ ...attribute });
  };

  const editAttributeValue = () => {
    onEditAttributeValue(formData, index);
    props.closeModal();
  };

  const {
    displayOrder,
    pricePercentageAdjustment,
    priceAdjustment,
    name,
  } = formData;
  return (
    <Fragment>
      <PanelBody>
        <FormRow>
          <PrimaryTextbox
            name="name"
            value={name}
            placeholder="Name"
            onChange={handleInputChange}
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="priceAdjustment"
            placeholder="Price adjustment"
            onChange={handleInputChange}
            value={priceAdjustment ? priceAdjustment : ""}
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="pricePercentageAdjustment"
            placeholder="Price percentage adjustment"
            onChange={handleInputChange}
            value={pricePercentageAdjustment ? pricePercentageAdjustment : ""}
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="displayOrder"
            placeholder="Display Order"
            onChange={handleInputChange}
            value={displayOrder ? displayOrder : ""}
          />
        </FormRow>
      </PanelBody>
      <PanelFooter>
        <ButtonPrimary onClick={() => editAttributeValue()} size="xs">
          {index || index === 0 ? "Update" : "Add"}
        </ButtonPrimary>
      </PanelFooter>
    </Fragment>
  );
}
